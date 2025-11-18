# Business Card Management System (Backend)

This repository contains the backend API for the Business Card Management System. It is built using **.NET 9** and follows **Domain-Driven Design (DDD)** principles with **Clean Architecture**.

## 🏗️ Architecture & Template

This project was scaffolded using the **Ahmad Awwad .NET Clean Architecture Template**. You can find the original template on [GitHub](https://github.com/aawwad172/dotnet-template).

The solution is organized into the following layers:

  - **Domain:** Core business entities (`BusinessCard`), value objects, enums (`Gender`), and repository interfaces.
  - **Application:** Use cases, CQRS (Command/Query handlers), validators (FluentValidation), and application services (Import/Export logic).
  - **Infrastructure:** Database context (EF Core), migrations, repository implementations, and file parsers (CSV, XML, QR/Image).
  - **Presentation:** The Web API, minimal apis, and middlewares.

### Key Features Inherited from Template

  * **CQRS & MediatR:** Separation of read and write operations.
  * **UnitOfWork:** Transaction management.
  * **Global Exception Handling:** Centralized middleware for standardized `ApiResponse`.
  * **Husky:** Pre-commit hooks to ensure code quality.
  * **Makefile:** Shortcuts for running, building, and database operations.

-----

## ⚠️ Note on Authentication & Authorization

The project includes a fully implemented generic **Authentication and Authorization** system (JWT-based) inherited from the base template.

> **Note:** While the infrastructure for Identity and JWT is present in the codebase, it is **not currently enforced** on the Business Card endpoints for the scope of this specific assignment. The code has been preserved for future scalability but is currently unused.

-----

## 🚀 Getting Started

### 1\. Prerequisites

  * .NET 9 SDK
  * Docker (for the database)
  * Make (optional, for running shortcut commands)

### 2\. Database Setup (PostgreSQL)

This project uses PostgreSQL. You can run a local instance using the official Docker image.

Run the following command to start the database with the default credentials configured in the application (`appsettings.json`):

```bash
docker run --name progress-soft-db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres
```

  * **Username:** `postgres`
  * **Password:** `postgres`
  * **Port:** `5432`

### 3\. Applying Migrations

The migration files are already generated and included in the repository. You only need to apply them to your running database instance.

If you have `Make` installed:

```bash
make database-update
```

Or using the .NET CLI:

```bash
dotnet ef database update --project src/ProgressSoft.Infrastructure --startup-project src/ProgressSoft.Presentation.API
```

### 4\. Running the Application

```bash
make run
```

*The API will be available at `https://localhost:7079` (or similar, check console output).*

-----

## 📡 API Documentation

The API provides endpoints for managing business cards, including CRUD operations and bulk import/export capabilities.

### 📇 Business Cards CRUD

| Method | Endpoint | Description | Request Body / Params |
| :--- | :--- | :--- | :--- |
| **GET** | `/api/business-cards` | Retrieve a paginated list of business cards. | `?page=1&pageSize=10` (Optional: Filtering params) |
| **GET** | `/api/business-cards/{id}` | Get detailed information for a single business card. | Path: `id` (Integer) |
| **POST** | `/api/business-cards` | Create a new business card manually. | JSON: `{ "name": "...", "gender": "Male", ... }` |
| **PUT** | `/api/business-cards` | Update an existing business card. | JSON: `{ "id": 1, "name": "...", ... }` |
| **DELETE** | `/api/business-cards/{id}` | Delete a business card by ID. | Path: `id` (Integer) |

### 📂 Bulk Import & Parsing

The system supports importing business cards from various file formats.

| Method | Endpoint | Description | Supported Formats |
| :--- | :--- | :--- | :--- |
| **POST** | `/api/businesscards/import` | Upload a file to parse and save business cards. | **CSV** (Standard headers)<br>**XML** (Standard schema)<br>**Image** (QR Code containing vCard data) |

**Note on QR Import:** Uploading an image (`.png`, `.jpg`) containing a QR code will automatically detect the code, parse the vCard data, and save the record to the database.

### 📤 Export Data

Download the database records in standard file formats.

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| **GET** | `/api/business-cards/export` | Download all records as a `.csv` or `.xml` file. |

-----

## 🛠 Configuration

You can adjust the database connection string in `src/ProgressSoft.Presentation.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=5432;Database=ProgressSoftDb;User Id=postgres;Password=postgres;"
}
```

-----

## 🤝 Contributions

I would be more than happy to tke notes on both template level and task level despite the result of the (hiring process).

-----

**By Ahmad Awwad :)**