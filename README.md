# StudifyAPI

A high-performance, maintainable **.NET 10 Web API** designed for study tracking. It leverages **Vertical Slice Architecture** to ensure clean separation of concerns and rapid feature development.

## 🚀 Technology Stack
*   **Framework:** .NET 9
*   **Database:** MySQL (Entity Framework Core + Pomelo)

...

## 🚀 Deployment

The API is deployed to **Render** using Docker.

### Production Environment Variables
To run the production build, the following environment variables are required:

| Variable Key | Description |
| :--- | :--- |
| `ConnectionStrings__DefaultConnection` | The MySQL connection string for the Aiven database. |
| `JwtSettings__SecretKey` | A 32+ character random string for JWT signing. |

### Database
The application is configured to automatically run EF Core migrations upon startup, ensuring the database schema is kept in sync with the application code.
*   **Authentication:** JWT Bearer tokens with **Refresh Token Rotation**
*   **Mapping:** AutoMapper
*   **Testing:** xUnit, Moq, FluentAssertions
*   **Documentation:** Swagger / OpenAPI

## 🏗️ Architecture
This project follows a **Vertical Slice** pattern. Code is organized by feature (e.g., `Features/Auth/`, `Features/Users/`). Each slice encapsulates its own Controller, Service, Repository, Model, and DTOs.

## 🛠️ Getting Started

### Prerequisites
*   .NET 10 SDK
*   MySQL Server (local or remote)

### Setup
1.  **Clone the Repository**
    ```bash
    git clone https://github.com/rainyDayssss/StudifyAPI.git
    cd StudifyAPI
    git checkout main
    ```
2.  **Configure Connection String:** Update `StudifyAPI/appsettings.json` with your MySQL connection string.
3.  **Manage Secrets:**
    ```bash
    cd StudifyAPI/StudifyAPI
    dotnet user-secrets set "JwtSettings:SecretKey" "<your-secure-random-key>"
    ```
4.  **Database Migration:** Apply the schema changes.
    ```bash
    dotnet ef database update
    ```

### Running the Application
```bash
cd StudifyAPI/StudifyAPI
dotnet run
```
Access the Swagger UI at `http://localhost:5000/swagger`.

## 🧪 Testing
We use xUnit with Moq and FluentAssertions. To run the full test suite:
```bash
dotnet test
```

## 📋 Development Workflow
*   **Branching:** The primary branch is `main`. Always create feature branches from `main`.
*   **Migrations:** Whenever models change:
    ```bash
    dotnet ef migrations add <MigrationName>
    ```

## 📄 License
This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

## 🤝 Contributing
*   Follow the Vertical Slice folder structure.
*   Always add unit tests for new service logic.
*   Ensure all tests pass before submitting a PR to `main`.
