[![Build Status](https://img.shields.io/github/actions/workflow/status/username/taskmaster-api/d://github.com/username/taskse: MIT](https://img.shields.io/badge/License-MIT-blue.svge of Contents

About

Features

Getting Started

Usage

Technologies

Contributing

License

Contact

About
TaskMaster API is a RESTful Web API built with ASP.NET Core 9 and Entity Framework Core 9. It provides endpoints to manage tasks, projects, and users with full CRUD operations, designed for seamless integration with frontend apps or third-party services.

Features
CRUD operations for tasks, projects, and users

Entity Framework Core 9 integration with SQL Server support

Asynchronous programming for high performance

JWT-based authentication and authorization

Swagger/OpenAPI documentation for easy API exploration

Unit and integration tests included

Getting Started
Prerequisites
.NET 9 SDK

SQL Server or compatible database

Optional: Postman or any API client for testing

Installation
bash
# Clone the repository
git clone https://github.com/username/taskmaster-api.git

# Navigate to the project directory
cd taskmaster-api

# Restore dependencies
dotnet restore

# Update the connection string in appsettings.json
# Example:
# "ConnectionStrings": {
#   "DefaultConnection": "Server=localhost;Database=TaskMasterDb;Trusted_Connection=True;"
# }

# Apply database migrations
dotnet ef database update

# Run the API
dotnet run
The API will be available at https://localhost:5001 by default.

Usage
Access Swagger UI at https://localhost:5001/swagger to explore and test API endpoints.

Use JWT tokens to authenticate protected endpoints.

Example request to get all tasks:

text
GET /api/tasks
Authorization: Bearer {your_token}
Technologies
.NET 9

Entity Framework Core 9

SQL Server

Swagger / Swashbuckle

JWT Authentication

Contributing
Contributions are welcome! Please follow these steps:

Fork the repository

Create a new branch (git checkout -b feature/YourFeature)

Commit your changes (git commit -m 'Add some feature')

Push to the branch (git push origin feature/YourFeature)

Open a Pull Request

Please see CONTRIBUTING.md for detailed guidelines.
