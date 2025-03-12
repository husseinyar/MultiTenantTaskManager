Multi-Tenant Task Manager
Overview
The Multi-Tenant Task Manager is a web application built using ASP.NET Core and Entity Framework Core. It allows multiple tenants (organizations or users) to manage their tasks independently within a shared application environment. The application leverages Microsoft's Identity framework for user authentication and authorization, and it uses a multi-tenant architecture to ensure data isolation between tenants.

Features
Multi-Tenant Architecture: Each tenant has its own isolated data, ensuring that tasks and user information are not shared between tenants.

User Authentication: Built-in user authentication and authorization using ASP.NET Core Identity.

Task Management: Tenants can create, assign, and manage tasks.

Entity Framework Core: Utilizes EF Core for database operations and management.

Customizable User Roles: Extendable user roles and permissions to fit different tenant needs.

Technologies Used
ASP.NET Core: A cross-platform, high-performance framework for building modern, cloud-based, and internet-connected applications.

Entity Framework Core: An object-relational mapper (ORM) that enables .NET developers to work with a database using .NET objects.

Microsoft Identity: Provides authentication and authorization services for ASP.NET Core applications.

SQL Server: The database used to store tenant, user, and task information.

Getting Started
Prerequisites
.NET 6 SDK

Visual Studio 2022 or Visual Studio Code

SQL Server

Installation
Clone the Repository

bash
Copy
git clone https://github.com/husseinyar/multi-tenant-task-manager.git
cd multi-tenant-task-manager
Set Up the Database

Update the appsettings.json file with your SQL Server connection string.

Run the following commands to apply migrations and seed the database:

bash
Copy
dotnet ef database update
Run the Application

bash
Copy
dotnet run
The application should now be running at https://localhost:5001.

Project Structure
Models: Contains the domain models for Tenant, TaskItem, and ApplicationUser.

Data: Contains the ApplicationDbContext and database configurations.

Migrations: Contains the Entity Framework Core migrations for database schema updates.

Controllers: Contains the ASP.NET Core controllers for handling HTTP requests.

Views: Contains the Razor views for the user interface (if applicable).

wwwroot: Contains static files like CSS, JavaScript, and images.

Database Schema
Tenants: Stores information about each tenant.

Tasks: Stores task items associated with a specific tenant.

ApplicationUsers: Stores user information, including their associated tenant.

Customizing the Application
Adding New Features: You can extend the application by adding new models, controllers, and views as needed.

Customizing User Roles: Modify the ApplicationUser class and Identity configurations to add custom roles and permissions.

Multi-Tenant Strategies: The current implementation uses a shared database with tenant isolation via foreign keys. You can explore other multi-tenant strategies like separate databases or schemas.

Contributing
Contributions are welcome! Please follow these steps to contribute:

Fork the repository.

Create a new branch (git checkout -b feature/YourFeatureName).

Commit your changes (git commit -m 'Add some feature').

Push to the branch (git push origin feature/YourFeatureName).

Open a pull request.
