# BierAlyzer API Documentation

## Overview

The BierAlyzer API provides a RESTful interface for the BierAlyzer application, allowing users to manage events, track drink consumption, and manage their profiles. It also includes administrative features for managing users, drinks, and events globally.

The API is built with .NET Core and uses JWT (JSON Web Tokens) for authentication.

## Getting Started

### Prerequisites

- .NET Core SDK 2.1 or later
- MySql Server

### Running the API

1.  Navigate to the `BierAlyzer.Api` directory.
2.  Restore dependencies: `dotnet restore`
3.  Run the application: `dotnet run`

The API will start (default port usually 5000 or 5001).

### Swagger UI

Interactive API documentation is available via Swagger UI.
Once the API is running, navigate to:

`http://localhost:5000/swagger`

This interface allows you to explore endpoints, see request/response models, and test API calls directly.

## Authentication

The API uses Bearer Token authentication.

1.  **Register:** Create a new account using `POST /api/auth/register`.
2.  **Login:** Obtain an access token using `POST /api/auth/token` with your email and password.
3.  **Authorize:** Include the returned `accessToken` in the `Authorization` header of subsequent requests:
    `Authorization: Bearer <your_access_token>`
4.  **Refresh:** When the access token expires, use the `refreshToken` to get a new pair using `POST /api/auth/refresh`.

## Key Resources

### User (`/api/user`)
-   **Profile:** Get or update your user profile (Username, Origin, Password).

### Event (`/api/event`)
-   **List:** Get all events you created or joined.
-   **Create/Update:** Manage your own private events.
-   **Join:** Join an event using a unique 4-character code or by public ID.
-   **Book Drink:** Log a drink consumption for an active event.
-   **Status:** Change event status (Open/Closed).

### Management (`/api/management`)
*Requires Admin privileges.*
-   **Users:** View and manage all registered users (Enable/Disable/Edit).
-   **Drinks:** Manage the global catalog of available drinks.
-   **Events:** View and delete any event in the system.

## Error Handling

Responses generally follow a standard format containing a `Result` object:

```json
{
  "Result": {
    "Success": false,
    "Status": "ErrorStatus",
    "Message": "Error details"
  }
}
```

Common HTTP Status Codes:
-   `200 OK`: Success
-   `400 Bad Request`: Validation error or business logic failure.
-   `401 Unauthorized`: Missing or invalid token.
-   `403 Forbidden`: Insufficient permissions (e.g., non-admin accessing management).
-   `404 Not Found`: Resource not found.
-   `500 Internal Server Error`: Unexpected server error.
