using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Data;

namespace fyp
{
    public class NotificationHub : Hub
    {
        // Concurrent dictionary to store userId to connectionId mapping
        private static readonly ConcurrentDictionary<int, string> UserConnections = new ConcurrentDictionary<int, string>();
        // Define methods that clients can call
        public void SendMessage(string message)
        {
            Clients.All.broadcastMessage(message); // Notify all connected clients
        }

        // This is where you handle the announcement notification
        public void ReceiveAnnouncement(string title, string content)
        {
            Clients.All.receiveAnnouncement(title, content);
        }

        // Get a connection ID for a specific user
        public static string GetConnectionId(int userId)
        {
            if (UserConnections.TryGetValue(userId, out string connectionId))
            {
                return connectionId;
            }
            return null;
        }

        public override Task OnConnected()
        {
            string connectionId = Context.ConnectionId;
            int userId = Convert.ToInt32(Context.QueryString["userId"]);

            // Map the userId to the connectionId
            UserConnections[userId] = connectionId;

            // Check for undelivered messages
            DeliverUndeliveredMessages(userId, connectionId);
            // Validate and notify if overdue loans exist
            DataTable overdueLoans = GetOverdueLoans(userId);
            if (overdueLoans != null && overdueLoans.Rows.Count > 0)
            {
                NotifyOverdueLoans(userId);
            }
            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            int userId = UserConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (userId != 0)
            {
                UserConnections.TryRemove(userId, out _);
            }

            return base.OnDisconnected(stopCalled);
        }

        public void ReceiveInbox(int userId, string title, string content, string sendAt)
        {
            System.Diagnostics.Debug.WriteLine($"ReceiveInbox triggered for UserId {userId}: {title}, {content}, {sendAt}");
            if (UserConnections.TryGetValue(userId, out string connectionId))
            {
                Clients.Client(connectionId).broadcastInbox(title, content, sendAt);
            }
        }

        public void ReceiveOver(int userId, string title, string content)
        {
            System.Diagnostics.Debug.WriteLine($"ReceiveInbox triggered for UserId {userId}: {title}, {content}");
            if (UserConnections.TryGetValue(userId, out string connectionId))
            {
                Clients.Client(connectionId).overdueNotification(title, content);
            }
        }

        private void DeliverUndeliveredMessages(int userId, string connectionId)
        {
            string selectQuery = "SELECT InboxId, InboxTitle, InboxContent, SendAt FROM Inbox WHERE UserId = @UserId AND IsDelivered = 0";
            DataTable result = DBHelper.ExecuteQuery(selectQuery, new object[] { "@UserId", userId });

            foreach (DataRow row in result.Rows)
            {
                int inboxId = Convert.ToInt32(row["InboxId"]);
                string title = row["InboxTitle"].ToString();
                string content = row["InboxContent"].ToString();
                DateTime sendAt = Convert.ToDateTime(row["SendAt"]);

                // Send the message
                Clients.Client(connectionId).broadcastInbox(title, content, sendAt.ToString("yyyy-MM-dd HH:mm:ss"));

                // Mark as delivered
                string updateQuery = "UPDATE Inbox SET IsDelivered = 1 WHERE InboxId = @InboxId";
                DBHelper.ExecuteNonQuery(updateQuery, new object[] { "@InboxId", inboxId });
            }
        }

        public DataTable GetOverdueLoans(int userId)
        {
            string query = @"
        SELECT 
            L.LoanId,
            L.PatronId,
            L.EndDate,
            DATEDIFF(DAY, L.EndDate, GETDATE()) AS OverdueDays,
            ISNULL(P.TotalFine, 0) AS TotalFine,
            BC.ISBN,
            ISNULL(T.TrustScore, 0) AS TrustScore
        FROM 
            Loan L
        INNER JOIN 
            BookCopy BC ON L.BookCopyId = BC.BookCopyId
        LEFT JOIN 
            Punishment P ON L.LoanId = P.LoanId
        LEFT JOIN 
            Trustworthy T ON L.PatronId = T.PatronId
        WHERE 
            L.Status = 'loaning' 
            AND DATEDIFF(DAY, L.EndDate, GETDATE()) > 0 
            AND L.PatronId = (SELECT PatronId FROM Patron WHERE UserId = @UserId)";

            return DBHelper.ExecuteQuery(query, new object[] { "@UserId", userId });
        }

        public void NotifyOverdueLoans(int userId)
        {
            DataTable overdueLoans = GetOverdueLoans(userId);

            if (overdueLoans == null || overdueLoans.Rows.Count == 0)
            {
                // No overdue loans, no action needed
                System.Diagnostics.Debug.WriteLine($"User {userId} has no overdue loans.");
                return;
            }

            foreach (DataRow loan in overdueLoans.Rows)
            {
                string title = "Overdue Loan Reminder";
                string content = $"You have an overdue loan for Book ISBN: {loan["ISBN"]}. " +
                                 $"It is overdue by {loan["OverdueDays"]} days. Please return it as soon as possible.";

                // Send notification using SignalR
                ReceiveOver(userId, title, content);
            }
        }

    }
}