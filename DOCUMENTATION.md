# BierAlyzer Project Documentation

## 1. Project Overview

BierAlyzer is a comprehensive application for tracking drink consumption during events. It allows users to join events, log drinks, and view statistics. The system supports different user roles (Admin, User), event management (Private, Public), and customizable drink definitions.

This documentation is intended for an AI or developer to re-implement the system from scratch. It describes the database structure and a complete REST API specification that covers all functionalities required for a frontend application (Web, Mobile, etc.).

**Note:** This specification describes a *complete* API that may supersede partial existing implementations. It is designed to be fully compatible with the existing database schema.

---

## 2. Database Schema

The database relies on a relational model (SQL). Below are the entity definitions.

### 2.1. User

Represents a registered user of the system.

| Column | Type | Description |
| :--- | :--- | :--- |
| `UserId` | GUID (PK) | Unique identifier for the user. |
| `Username` | String | Display name of the user. |
| `Mail` | String | User's email address (used for login). |
| `Origin` | String | Optional origin/location of the user. |
| `Type` | Enum/Int | Role: `0` (User), `1` (Admin). |
| `Hash` | String | Password hash. |
| `Salt` | String | Salt used for hashing. |
| `Enabled` | Boolean | Whether the account is active. |
| `Created` | DateTime | Account creation timestamp. |
| `Modified` | DateTime | Last modification timestamp. |
| `LastLogin` | DateTime | Timestamp of the last successful login. |

### 2.2. Event

Represents a gathering or session where drinks are tracked.

| Column | Type | Description |
| :--- | :--- | :--- |
| `EventId` | GUID (PK) | Unique identifier for the event. |
| `OwnerId` | GUID (FK) | Reference to the `User` who created the event. |
| `Name` | String | Name of the event. |
| `Description` | String | Description/details. |
| `Code` | String | Unique code for joining the event (e.g., for private events). |
| `Type` | Enum/Int | `0` (Private), `1` (Public), `2` (Hidden). |
| `Start` | DateTime | Scheduled start time. |
| `End` | DateTime | Scheduled end time. |
| `Created` | DateTime | Creation timestamp. |
| `Modified` | DateTime | Last modification timestamp. |

**Computed Properties:**
*   `Status`: Derived from `Start` and `End`.
    *   `Open`: Current time is between Start and End.
    *   `NotYet`: Start time is in the future.
    *   `Closed`: End time is in the past.

### 2.3. Drink

Represents a type of beverage available for consumption.

| Column | Type | Description |
| :--- | :--- | :--- |
| `DrinkId` | GUID (PK) | Unique identifier for the drink. |
| `OwnerId` | GUID (FK) | Reference to `User` (if user-specific) or null (global). |
| `Name` | String | Name of the drink (e.g., "Beer"). |
| `Amount` | Double | Volume in liters (e.g., 0.5). |
| `Percentage` | Double | Alcohol percentage (e.g., 5.0). |
| `Visible` | Boolean | Whether the drink is selectable in lists. |
| `Created` | DateTime | Creation timestamp. |
| `Modified` | DateTime | Last modification timestamp. |

**Computed Properties:**
*   `AlcoholAmount`: Pure alcohol content in grams/units. Typically calculated as: `0.8 * (Amount * 1000) * (Percentage / 100)`.

### 2.4. DrinkEntry

Represents a single consumption record (a specific user drinking a specific drink at a specific event).

| Column | Type | Description |
| :--- | :--- | :--- |
| `EntryId` | GUID (PK) | Unique identifier for the entry. |
| `UserId` | GUID (FK) | The user who consumed the drink. |
| `EventId` | GUID (FK) | The event where it was consumed. |
| `DrinkId` | GUID (FK) | The drink consumed. |
| *Date* | *DateTime* | *Implicitly tracked via creation time (if added to schema) or derived.* |

### 2.5. UserEvent

Link table representing a user's membership in an event.

| Column | Type | Description |
| :--- | :--- | :--- |
| `UserId` | GUID (PK, FK) | The user. |
| `EventId` | GUID (PK, FK) | The event. |

---

## 3. API Specification

The API should follow RESTful principles. All endpoints (except Auth) require a valid Bearer Token (JWT) in the `Authorization` header.

### 3.1. Authentication (`/api/auth`)

| Method | Endpoint | Description | Request Body | Response |
| :--- | :--- | :--- | :--- | :--- |
| `POST` | `/login` | Authenticate user. | `{ "mail": "...", "password": "..." }` | `{ "accessToken": "...", "refreshToken": "..." }` |
| `POST` | `/register` | Register new user. | `{ "username": "...", "mail": "...", "password": "..." }` | `200 OK` |
| `POST` | `/refresh` | Refresh access token. | `{ "refreshToken": "..." }` | `{ "accessToken": "...", "refreshToken": "..." }` |

### 3.2. Events (`/api/events`)

| Method | Endpoint | Description | Request Body | Response |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/` | List relevant events (Public & Joined). | - | List of `Event` objects. |
| `POST` | `/` | Create a new event. | `{ "name": "...", "start": "...", "end": "...", "description": "...", "type": 0 }` | Created `Event` object. |
| `GET` | `/{id}` | Get event details, including drink usage stats and user list. | - | `EventDetail` object. |
| `PUT` | `/{id}` | Update event details (Owner only). | `{ "name": "...", "start": "...", "end": "...", "description": "..." }` | Updated `Event`. |
| `DELETE` | `/{id}` | Delete an event (Owner only). | - | `200 OK` |
| `POST` | `/{id}/join` | Join a public event. | - | `200 OK` |
| `POST` | `/{id}/leave` | Leave an event. | - | `200 OK` |
| `POST` | `/join/{code}` | Join a private event using its unique code. | - | `Event` object. |
| `PUT` | `/{id}/status` | Force update status (Open/Close). Updates Start/End times implicitly. | `{ "status": "Open" \| "Closed" }` | `200 OK` |

**Event Detail Response Model:**
The `GET /events/{id}` response should include:
- Basic Event Info (Name, Description, Times, Code).
- `CurrentUserStatus`: Total liters/alcohol consumed by the requesting user in this event.
- `Leaderboard`: List of participants sorted by consumption.
- `DrinkUsage`: Aggregate count of each drink type consumed.

### 3.3. Consumption (`/api/consumption`)

| Method | Endpoint | Description | Request Body | Response |
| :--- | :--- | :--- | :--- | :--- |
| `POST` | `/{eventId}` | Log a drink for the current user. | `{ "drinkId": "..." }` | `200 OK` |
| `DELETE` | `/{entryId}` | Undo a drink entry (optional feature). | - | `200 OK` |

### 3.4. Drinks (`/api/drinks`)

| Method | Endpoint | Description | Request Body | Response |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/` | List all visible drinks. | - | List of `Drink` objects. |
| `POST` | `/` | Create a new custom drink. | `{ "name": "...", "amount": 0.5, "percentage": 5.0 }` | Created `Drink`. |
| `PUT` | `/{id}` | Update a drink (Owner only). | `{ "name": "...", ... }` | Updated `Drink`. |
| `DELETE` | `/{id}` | Delete a drink (if not used). | - | `200 OK` |

### 3.5. Users (`/api/users`)

| Method | Endpoint | Description | Request Body | Response |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/me` | Get current user profile. | - | `UserProfile` object. |
| `PUT` | `/me` | Update profile or password. | `{ "username": "...", "password": "..." }` | `200 OK` |
| `GET` | `/` | (Admin) List all users. | - | List of `User`. |
| `PUT` | `/{id}/toggle-status` | (Admin) Enable/Disable user account. | - | `200 OK` |


## 4. Business Logic & Implementation Details

To ensure consistency with the original design, the following business rules must be implemented.

### 4.1. Event Status Management

The `Status` of an event is technically a derived property based on `Start` and `End` times compared to `DateTime.Now`. However, users can manually "Open" or "Close" an event.

- **Set to OPEN:**
    - If `Start` is in the future, set `Start` to `Now - 1 minute`.
    - Set `End` to `Now + 1 day` (or another future duration).
    - *Effect:* The event becomes active immediately.

- **Set to CLOSED:**
    - Set `End` to `Now - 1 minute`.
    - If `Start` was after `End`, set `Start` to `Today` (start of day) to ensure consistency.
    - *Effect:* The event ends immediately.

### 4.2. Alcohol Calculation

The system calculates the pure alcohol content of a drink to generate statistics.

**Formula:**
```csharp
AlcoholAmount (grams) = 0.8 * (Amount_in_Liters * 1000) * (Percentage / 100)
```
*   **0.8**: Density of alcohol (approx).
*   **Amount**: Volume in Liters.
*   **Percentage**: Alcohol by volume (ABV).

### 4.3. Drink Visibility & Ownership

- **Global Drinks:** Drinks created by Admins or system defaults (where `OwnerId` is null, if applicable, or managed via specific Admin flags). Visible to everyone.
- **User Drinks:** Drinks created by a user (`OwnerId` = User).
- **Visibility:** Drinks have a `Visible` flag. If `False`, they should not appear in selection lists but historical data remains valid.

### 4.4. Frontend Implementation Recommendations

Any frontend (Web, Mobile) consuming this API should:
1.  **Store Tokens:** Securely store `AccessToken` and `RefreshToken`.
2.  **Handle 401s:** Automatically attempt to use the `RefreshToken` when an API call returns `401 Unauthorized`.
3.  **Event Polling:** For live event views, poll the `GET /events/{id}` endpoint periodically to update leaderboards in near real-time.


## 5. Legacy Authentication & Data Migration

To ensure existing users can log in to a re-implemented system, the authentication mechanism must replicate the legacy hashing algorithm used to generate the `Hash` and `Salt` stored in the database.

### 5.1. Algorithm Details

The system uses **MD5** for hashing. While MD5 is not cryptographically secure by modern standards, it is required to match the existing data.

*   **Hash Format:** All hashes are stored as **Uppercase Hexadecimal Strings** (e.g., `5D41402ABC4B2A76B9719D911017C592`).
*   **Text Encoding:** UTF-8.

### 5.2. Salt Generation
When creating a new user, a salt is generated as follows:
```csharp
Salt = MD5(Guid.NewGuid().ToString())
```

### 5.3. Password Hashing
To verify a password against the stored `Hash` and `Salt`:

1.  Construct the input string: `Salt + Password + Salt`.
2.  Compute the MD5 hash of this string.
3.  Compare the result with the stored `Hash`.

**Pseudocode:**
```
function VerifyPassword(inputPassword, storedSalt, storedHash):
    inputString = storedSalt + inputPassword + storedSalt
    computedHash = MD5(inputString).toHexString().toUpperCase()
    return computedHash == storedHash
```

## 6. Configuration & Environment Setup

This section describes how the existing application is configured, which serves as a baseline for the new implementation.

### 6.1. Database Connection
The application uses **MySQL** (via `Pomelo.EntityFrameworkCore.MySql`).
The connection string is typically stored in `appsettings.json` under `ConnectionStrings:Database`.

**Format:**
```json
{
  "ConnectionStrings": {
    "Database": "Server=localhost;database=bieralyzer_online;uid=USER;pwd=PASSWORD;"
  }
}
```

### 6.2. Application Settings
*   **Session Management:** The legacy web app uses server-side sessions with a **72-hour** idle timeout and HTTP-only cookies.
*   **Static Files:** Served from `wwwroot`, including bundled CSS/JS.

### 6.3. Hardcoded Configuration
Some settings in the legacy code are hardcoded and should be moved to a configuration file (like `appsettings.json`) in the new implementation.

*   **Known Origins:** A list of universities/locations used for the "Origin" dropdown is currently hardcoded in `BierAlyzerWeb.Helper.SharedProperties.KnownOrigins`. This includes values like "FH Aachen", "TU Berlin", "ETH Zürich", etc. The new implementation should preferably load these from a database table or a flexible configuration source.
