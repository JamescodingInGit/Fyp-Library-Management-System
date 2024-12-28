﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace fyp
{
    public partial class ReturnViewDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int loanId;
            if (int.TryParse(Request.QueryString["LoanId"], out loanId))
            {
                LoadLoanDetails(loanId);
            }
            else
            {
                Response.Redirect($"ReturnBookManagement.aspx");
            }
        }

        private void LoadLoanDetails(int loanId)
        {
            string query = @"
                    SELECT 
                        L.LoanId,
                        BC.BookCopyImage,
                        B.BookTitle,
                        A.AuthorName,
                        BC.PublishDate,
                        BC.PublishOwner,
                        L.StartDate,
                        L.EndDate,
                        L.IsApprove,
                        L.LatestReturn,
                        L.Status,
                        P.TotalFine,
                        P.DatePayed
                    FROM Loan L
                    JOIN BookCopy BC ON L.BookCopyId = BC.BookCopyId
                    JOIN Book B ON BC.BookId = B.BookId
                    JOIN BookAuthor BA ON B.BookId = BA.BookId
                    JOIN Author A ON BA.AuthorId = A.AuthorId
                    LEFT JOIN Punishment P ON L.LoanId = P.LoanId
                    WHERE L.LoanId = @LoanId";

            object[] selectParams = {
                "@LoanId", loanId
            };
            DataTable rt = fyp.DBHelper.ExecuteQuery(query, selectParams);

            if (rt.Rows.Count > 0)
            {
                DataRow row = rt.Rows[0]; // Get the first row

                // Book Image
                if (row["BookCopyImage"] != DBNull.Value)
                {
                    byte[] imageBytes = (byte[])row["BookCopyImage"];
                    string base64Image = Convert.ToBase64String(imageBytes);
                    bookImage.Src = $"data:image/png;base64,{base64Image}";
                }

                // Book Details
                lblTitle.InnerText = row["BookTitle"].ToString();
                lblPublishDate.InnerText = Convert.ToDateTime(row["PublishDate"]).ToString("dd MMM yyyy");
                lblPublishOwner.InnerText = row["PublishOwner"].ToString();
                lblStatus.InnerText = row["Status"].ToString();
                lblStatus.Attributes["class"] = $"status {row["Status"].ToString().ToLower()}";
                lblStartDate.InnerText = Convert.ToDateTime(row["StartDate"]).ToString("dd MMM yyyy hh:mm tt");
                lblEndDate.InnerText = Convert.ToDateTime(row["EndDate"]).ToString("dd MMM yyyy hh:mm tt");

                lblApproval.InnerText = (row["IsApprove"] != DBNull.Value && (bool)row["IsApprove"]) ? "Approved" : "Approving";

                if (row["LatestReturn"] != DBNull.Value && row["LatestReturn"] != null)
                {
                    lblLatestReturn.InnerText = Convert.ToDateTime(row["LatestReturn"]).ToString("dd MMM yyyy hh:mm tt");
                }
                else
                {
                    lblLatestReturn.InnerText = "Not Return Book Yet"; // Or any placeholder text
                }

                // Penalty Details
                if (row["TotalFine"] != DBNull.Value)
                {
                    penaltySection.Visible = true;
                    lblTotalFine.InnerText = $"RM{row["TotalFine"]}";
                    if (row["DatePayed"] != DBNull.Value && row["DatePayed"] != null)
                    {
                        lblDatePayed.InnerText = Convert.ToDateTime(row["DatePayed"]).ToString("dd MMM yyyy hh:mm tt");
                    }
                    else
                    {
                        lblDatePayed.InnerText = "Not Paid Yet"; // Or any placeholder text
                    }

                }
                else
                {
                    penaltySection.Visible = false; // Hide penalty section if no punishment exists
                }
            }
            else
            {
                // Show an alert message for no data
                string script = "alert('No loan details found for the given LoanId.');";
                ClientScript.RegisterStartupScript(this.GetType(), "NoDataAlert", script, true);
            }

        }
        protected void logOut_Click(object sender, EventArgs e)
        {
            Session["ReturnUserId"] = null;

            Response.Redirect("ReturnBookManagement.aspx");
        }


    }
}