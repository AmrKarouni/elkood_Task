TaskMaster API is a RESTful Web API built with ASP.NET Core 9 and Entity Framework Core 9. It provides endpoints to manage tasks, task categories and users with full CRUD operations, designed for seamless integration with frontend apps or third-party services.

- Features
  - CRUD operations for tasks, task categories and users
  - Entity Framework Core 9 integration with SQL Server support
  - Asynchronous programming for high performance
  - JWT-based authentication and authorization
  - Swagger documentation for easy API exploration
  - Unit and integration tests are not included -- due to short time frame but the code base in the app services were designed to be testable since all the dependencies area mockable.

- Prerequisites
  - .NET 9 SDK
  - SQL Server or compatible database
  - Optional: Postman or any API client for testing

Installation
- Clone the repository
git clone [https://github.com/username/taskmaster-api.git](https://github.com/AmrKarouni/elkood_Task)

- Navigate to the project directory
cd taskmaster-api

- Restore dependencies
dotnet restore

- Apply database migrations
dotnet ef database update
which will create the database and data seeding will run automatically on first run.

- Run the API
The API will be available at [https://localhost:5001](https://localhost:7001/) by default.

- Usage
Access Swagger UI at [https://localhost:5001/swagger](https://localhost:7001/index.html) to explore and test API endpoints.

Use JWT tokens to authenticate protected endpoints based on the project logic.

- Default app admin credentials:
Username: Owner
Password: Aa@123456

you can find the relevant credentials in the static class "SeedData" in the Web application layer.
