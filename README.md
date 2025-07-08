# BierAlyzer API

A RESTful ASP.NET Core 6 Web API for BierAlyzer, featuring Swagger documentation, Clean Architecture, and automated tests.

## Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Build & Run

```sh
cd BierAlyzer.Api
 dotnet build
 dotnet run
```

The API will be available at `https://localhost:5001` (or as configured).

## Swagger UI

Visit [https://localhost:5001/swagger](https://localhost:5001/swagger) for interactive API documentation.

## Testing

```sh
cd BierAlyzer.Api.Tests
 dotnet test
```

Test coverage is collected via coverlet.

## Project Structure
- `Controllers/` – API endpoints
- `Models/` – Domain models
- `DTOs/` – Data transfer objects
- `Services/` – Business logic
- `Repositories/` – Data access
- `Mappings/` – AutoMapper profiles

## Code Style
- Enforced via `.editorconfig` and StyleCop (add StyleCop.Analyzers if desired)

## Logging & Error Handling
- Logging via `ILogger<T>`
- Global exception handling middleware (to be implemented)
