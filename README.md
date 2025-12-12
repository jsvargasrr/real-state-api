# Real Estate API - Backend

## Overview

REST API developed with **.NET 8** for managing real estate properties. This project demonstrates advanced software engineering practices as a technical test for a Senior .NET Developer position.

## Architecture

The project follows **Hexagonal Architecture** (Ports & Adapters) with strict layer separation:

```
src/
├── RealEstate.Domain/        # Core business logic (Entities, Interfaces)
├── RealEstate.Application/   # Use cases and application services
├── RealEstate.Infrastructure/# External adapters (Database, EF Core)
└── RealEstate.Api/           # Entry point (Controllers, DI config)
```

### Layer Responsibilities

| Layer | Responsibility | Dependencies |
|-------|----------------|--------------|
| **Domain** | Entities, Repository interfaces, Business rules | None |
| **Application** | Use case handlers, DTOs, Validation | Domain only |
| **Infrastructure** | EF Core, PostgreSQL/SQL Server, Repository implementations | Domain, Application |
| **Api** | HTTP Controllers, Swagger, DI Configuration | All layers |

## Technologies Used

- **.NET 8** - Latest LTS version
- **Entity Framework Core 8** - ORM with Code First approach
- **PostgreSQL** (Development) / **SQL Server** (Production)
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Input validation
- **Swagger/OpenAPI** - API documentation
- **NUnit + Moq + FluentAssertions** - Unit testing

## Design Patterns Applied

- **Repository Pattern** - Data access abstraction
- **Unit of Work** - Transaction management
- **CQRS-lite** - Command/Query separation in handlers
- **DTOs** - Prevent domain leakage through API boundaries
- **Dependency Injection** - Loose coupling between layers

## API Endpoints

### Owners
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/owners` | List all owners |
| GET | `/api/owners/{id}` | Get owner by ID |
| POST | `/api/owners` | Create new owner |
| PUT | `/api/owners/{id}` | Update owner |
| DELETE | `/api/owners/{id}` | Delete owner |

### Properties
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/properties` | List properties (with filters & pagination) |
| GET | `/api/properties/{id}` | Get property by ID |
| POST | `/api/properties` | Create new property |
| PUT | `/api/properties/{id}` | Update property |
| PATCH | `/api/properties/{id}/price` | Change price (creates audit trace) |
| POST | `/api/properties/{id}/images` | Add image to property |

### Reservations
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/properties/{id}/reservations` | Get property reservations |
| POST | `/api/properties/{id}/reservations` | Create reservation |
| DELETE | `/api/properties/{id}/reservations/{reservationId}` | Cancel reservation |

## Filter Parameters (Properties)

| Parameter | Type | Description |
|-----------|------|-------------|
| `name` | string | Partial match on property name |
| `address` | string | Partial match on address |
| `minPrice` | decimal | Minimum price filter |
| `maxPrice` | decimal | Maximum price filter |
| `year` | int | Construction year |
| `ownerId` | Guid | Filter by owner |
| `page` | int | Page number (default: 1) |
| `pageSize` | int | Items per page (default: 10) |

## Local Setup

### Prerequisites

- .NET 8 SDK
- PostgreSQL
- IDE (Visual Studio, VS Code)

### Step 1: Clone and Navigate

```bash
cd src/RealEstate.Api
```

### Step 2: Configure Database Connection

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=realestate;Username=postgres;Password=your_password;"
  }
}
```

For SQL Server:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=RealEstateDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

### Step 3: Apply Migrations

```bash
dotnet ef database update
```

Or use the SQL scripts in `/database` folder.

### Step 4: Run the API

```bash
dotnet run --urls=http://0.0.0.0:3000
```

### Step 5: Access Swagger

Open in browser: `http://localhost:3000/swagger`

## Running Tests

```bash
cd tests/RealEstate.Tests
dotnet test --verbosity normal
```

### Test Coverage

- **32 unit tests** covering:
  - Domain entities (Property, Reservation)
  - Application handlers (Create, Update, List, ChangePrice)
  - Reservation system (Create, Get, Cancel)

### Test Frameworks

- **NUnit** - Test framework
- **Moq** - Mocking dependencies
- **FluentAssertions** - Readable assertions

## Project Structure

```
src/
├── RealEstate.Domain/
│   ├── Entities/
│   │   ├── Owner.cs
│   │   ├── Property.cs
│   │   ├── PropertyImage.cs
│   │   ├── PropertyTrace.cs
│   │   └── Reservation.cs
│   └── Interfaces/
│       ├── IOwnerRepository.cs
│       ├── IPropertyRepository.cs
│       ├── IReservationRepository.cs
│       └── IUnitOfWork.cs
│
├── RealEstate.Application/
│   ├── DTOs/
│   ├── Common/
│   ├── Mappings/
│   └── UseCases/
│       ├── CreateProperty/
│       ├── UpdateProperty/
│       ├── ListProperties/
│       ├── ChangePropertyPrice/
│       └── Reservations/
│
├── RealEstate.Infrastructure/
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations/
│   └── Repositories/
│
└── RealEstate.Api/
    ├── Controllers/
    ├── Program.cs
    └── appsettings.json
```

## Security Best Practices

- Input validation with FluentValidation
- DTOs to prevent over-posting attacks
- Parameterized queries (EF Core)
- No sensitive data in logs
- CORS configuration for frontend

## Performance Optimizations

- Database indexes on frequently queried columns
- Pagination for large datasets
- Async/await throughout the stack
- No N+1 query problems (proper Include statements)

## Author

Developed as a technical demonstration for Senior .NET Developer position.
