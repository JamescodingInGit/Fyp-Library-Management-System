**Final Year Project (FYP)**: **Online Library Management System** developed using **ASP.NET**.

---

# **Online Library Management System**

## **Overview**

The **Online Library Management System** is a modern web-based platform designed to revolutionize library management, improve user satisfaction, and optimize administrative processes. Built using **ASP.NET** and integrated with **SignalR** for real-time notifications, this system addresses challenges faced by traditional library systems, such as inefficient book searches, overdue penalties, and lack of user accountability.

The platform simplifies operations for both users and administrators by providing enhanced search functionality, automated penalty enforcement, user trust scoring, and streamlined administrative tools.

---

## **Key Features**

### **1. Enhanced Book Search Functionality**  
- Multi-functional search module with advanced filtering options.  
- Users can search for books by:
   - **Genre**, **Subject Matter**, **Language**, **Author**, and **Title**.  
- Promotes efficient resource discoverability, ensuring users can locate relevant materials quickly.  

### **2. Real-Time Notifications with SignalR**  
- Integrated **SignalR** for real-time updates and notifications.  
- Notifications include:
   - Overdue reminders.  
   - Important alerts for borrowing status, system updates, and changes.  
- Users receive notifications directly in their **inbox** within the system.  

### **3. User Trust Module**  
- Introduces a **credit scoring system** based on borrowing behavior.  
- Trust levels encourage responsible borrowing by:
   - Assigning scores based on the borrowing history.  
   - Enforcing restrictions on users with low trust scores.  
- Promotes fairness and accountability in resource usage.  

### **4. Automated Penalty Enforcement**  
- Automatic calculation and enforcement of **fines for overdue books**.  
- Sends push notifications to remind users of approaching deadlines.  
- Reduces manual effort for staff while ensuring timely returns.  

### **5. Admin and Staff Dashboard**  
- Admins and staff can efficiently manage the library through a comprehensive dashboard.  
- Dashboard capabilities include:  
   - **User Management**: Add, edit, and delete user accounts with precise permissions.  
   - **Book Management**: Add, update, or remove books from the catalog.  
   - **Overdue Monitoring**: View overdue items and impose penalties automatically.  
   - **Insights and Reports**: Access detailed analytics on patron behavior and resource usage.  
- Simplifies administrative tasks and enhances resource allocation.  

---

## **Objectives**

### **1. Enhance Book Search Functionality**  
- Implement an advanced book search module with filtering options.  
- Improve discoverability and ensure users can easily locate materials.

### **2. Promote Responsible Borrowing**  
- Introduce a user trust scoring system to encourage accountability.  
- Ensure fair access to resources and improve borrowing behavior.

### **3. Improve User Accountability**  
- Automate fine enforcement for overdue books.  
- Send real-time notifications using SignalR for updates and reminders.

### **4. Optimize Library Management**  
- Provide detailed insights into patron behavior and resource usage.  
- Simplify administrative tasks with a robust dashboard for staff.

---

## **Technologies Used**

- **Frontend**: HTML, CSS, JavaScript
- **Backend**: C# (ASP.NET Framework)  
- **Real-Time Notifications**: SignalR  
- **Database**: SQL Server Management Studio  
- **Tools**: Visual Studio

---

## **System Architecture**

1. **User Module**:  
   - Search for books using filters.  
   - Receive real-time notifications for overdue reminders and system updates.  
   - View borrowing history, penalties, and inbox notifications.

2. **Admin/Staff Module**:  
   - Manage user accounts, books, and borrowing history.  
   - Enforce penalties and send updates.  
   - View insights and reports via the dashboard.

3. **Real-Time Notification Module**:  
   - Use **SignalR** to send live notifications and messages.  
   - Enable inbox messaging for better communication.

---

## **Installation and Setup**

### **Prerequisites**
- Visual Studio 2019 or higher  
- SQL Server Management Studio 2019 or higher  
- .NET Framework  
- IIS (Internet Information Services) for deployment  

### **Steps to Run the Project**
1. Clone the repository:  
   ```bash
   git clone https://github.com/JamescodingInGit/FYP-Degree.git
   ```
2. Open the solution in **Visual Studio**.  
3. Configure the SQL Server database connection in `web.config`.  
4. Restore NuGet packages and build the project.  
5. Deploy to IIS or run the project locally using Visual Studio.  

---

## **Conclusion**
The **Online Library Management System** provides a comprehensive solution for modernizing library operations. By introducing advanced search capabilities, real-time notifications, and automated processes, the system enhances user satisfaction, promotes responsible borrowing, and simplifies administrative tasks for staff.

With features like the **user trust module**, real-time updates via **SignalR**, and an intuitive admin dashboard, this platform ensures an efficient and user-friendly experience for both library patrons and staff.

---
