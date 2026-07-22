# Agent Instructions: StudifyAPI

This is a .NET 10.0 Web API project using a Feature-based (Vertical Slice) folder structure.

## Architecture & Layout
- **Features Folder**: `StudifyAPI/Features/` contains the domain slices (Auth, Users, Friends, Pomodoro, Tasks, UserStreaks). Each slice generally encapsulates its own `Controller`, `Service`, `Repository`, `Model`, and `DTO`s.
- **Shared Folder**: `StudifyAPI/Shared/` contains cross-cutting concerns like `StudifyDbContext` (EF Core), custom domain exceptions, and the global `ExceptionMiddleware`.
- **Entrypoint**: `StudifyAPI/Program.cs` registers Dependency Injection (DI), EF Core, Authentication, and Swagger.

## Toolchain & Conventions
- **Framework**: .NET 10 (`net10.0`).
- **Database**: Entity Framework Core with **MySQL** (using `Pomelo.EntityFrameworkCore.MySql`). 
- **Secret Management**: Do not hardcode secrets. Local database passwords and JWT secrets should be managed via the native `.NET Secret Manager` (`dotnet user-secrets`), not `.env` files.
- **Authentication**: JWT Bearer tokens. Endpoints securely access the current user via claims (`User.FindFirst("userId")`).
- **Mapping**: AutoMapper is used for DTO-to-Model mappings. 
- **Error Handling**: Throw custom exceptions (e.g., `UserNotFoundException`) from Services. The `ExceptionMiddleware` catches them and formats standard JSON responses with appropriate HTTP status codes.
- **CORS**: Configured strictly to allow a React dev server at `http://localhost:5173`.

## Developer Commands
*Run these from the `StudifyAPI/StudifyAPI` directory containing the `.csproj`.*
- **Build**: `dotnet build`
- **Run**: `dotnet run`
- **Database Migrations**: `dotnet ef migrations add <MigrationName>` and `dotnet ef database update`

## Quirks & Notes
- **Testing**: There are currently no test projects or suites in this repository. 
- **Dependency Injection**: When adding a new Feature slice, manually register its Repository and Service in `Program.cs` (e.g., `builder.Services.AddScoped<IUserService, UserService>();`).
