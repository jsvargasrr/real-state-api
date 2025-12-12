# Real Estate Database - Entity Relationship Diagram

## Database Schema Diagram (ASCII)

```
┌─────────────────────────────────────────────────────────────────────────────────────────────────────────┐
│                                    REAL ESTATE DATABASE SCHEMA                                          │
│                                    Technical Test - Senior .NET Developer                               │
└─────────────────────────────────────────────────────────────────────────────────────────────────────────┘

                                              ┌─────────────────────┐
                                              │       OWNERS        │
                                              ├─────────────────────┤
                                              │ PK IdOwner (UUID)   │
                                              │    Name             │
                                              │    Address          │
                                              │    Photo            │
                                              │    Birthday         │
                                              │    CreatedAt        │
                                              │    UpdatedAt        │
                                              └──────────┬──────────┘
                                                         │
                                                         │ 1:N
                                                         │
                                              ┌──────────▼──────────┐
                                              │     PROPERTIES      │
                                              ├─────────────────────┤
                                              │ PK IdProperty (UUID)│
                                              │    Name             │
                                              │    Address          │
                                              │    Price (COP)      │
                                              │    CodeInternal (UQ)│
                                              │    Year             │
                                              │ FK IdOwner          │───────┐
                                              │    CreatedAt        │       │
                                              │    UpdatedAt        │       │
                                              └──────────┬──────────┘       │
                                                         │                  │
                         ┌───────────────────────────────┼───────────────┬──┘
                         │                               │               │
                         │ 1:N                           │ 1:N           │ 1:N
                         │                               │               │
              ┌──────────▼──────────┐         ┌──────────▼──────────┐   ┌▼─────────────────────┐
              │   PROPERTY_IMAGES   │         │   PROPERTY_TRACES   │   │     RESERVATIONS     │
              ├─────────────────────┤         ├─────────────────────┤   ├──────────────────────┤
              │ PK IdPropertyImage  │         │ PK IdPropertyTrace  │   │ PK IdReservation     │
              │ FK IdProperty       │         │ FK IdProperty       │   │ FK IdProperty        │
              │    File             │         │    DateSale         │   │    GuestName         │
              │    Enabled          │         │    Name             │   │    GuestEmail        │
              │    CreatedAt        │         │    Value            │   │    CheckIn           │
              └─────────────────────┘         │    Tax              │   │    CheckOut          │
                                              │    CreatedAt        │   │    Guests            │
                                              └─────────────────────┘   │    TotalPrice        │
                                                                        │    Status            │
                                                                        │    CreatedAt         │
                                                                        └──────────────────────┘
```

## Table Descriptions

### Owners (Propietarios)
Stores information about property owners.
| Column | Type | Description |
|--------|------|-------------|
| IdOwner | UUID | Primary key |
| Name | NVARCHAR(200) | Full name |
| Address | NVARCHAR(500) | Owner's address |
| Photo | NVARCHAR(500) | Photo URL (optional) |
| Birthday | DATETIME2 | Date of birth |
| CreatedAt | DATETIME2 | Creation timestamp |
| UpdatedAt | DATETIME2 | Last update timestamp |

### Properties (Propiedades)
Stores real estate property information.
| Column | Type | Description |
|--------|------|-------------|
| IdProperty | UUID | Primary key |
| Name | NVARCHAR(200) | Property name |
| Address | NVARCHAR(500) | Property location |
| Price | DECIMAL(18,2) | Price in COP (Colombian Pesos) |
| CodeInternal | NVARCHAR(50) | Unique internal code |
| Year | INT | Construction year |
| IdOwner | UUID | Foreign key to Owners |
| CreatedAt | DATETIME2 | Creation timestamp |
| UpdatedAt | DATETIME2 | Last update timestamp |

### PropertyImages (Imágenes de Propiedades)
Stores images associated with properties.
| Column | Type | Description |
|--------|------|-------------|
| IdPropertyImage | UUID | Primary key |
| IdProperty | UUID | Foreign key to Properties |
| File | NVARCHAR(500) | Image URL or path |
| Enabled | BIT | Is image active? |
| CreatedAt | DATETIME2 | Creation timestamp |

### PropertyTraces (Historial de Precios)
Tracks price change history for audit purposes.
| Column | Type | Description |
|--------|------|-------------|
| IdPropertyTrace | UUID | Primary key |
| IdProperty | UUID | Foreign key to Properties |
| DateSale | DATETIME2 | Date of price change |
| Name | NVARCHAR(200) | Description |
| Value | DECIMAL(18,2) | New price |
| Tax | DECIMAL(18,2) | Associated tax |
| CreatedAt | DATETIME2 | Creation timestamp |

### Reservations (Reservaciones)
Stores property reservations.
| Column | Type | Description |
|--------|------|-------------|
| IdReservation | UUID | Primary key |
| IdProperty | UUID | Foreign key to Properties |
| GuestName | NVARCHAR(200) | Guest full name |
| GuestEmail | NVARCHAR(200) | Guest email |
| CheckIn | DATETIME2 | Check-in date |
| CheckOut | DATETIME2 | Check-out date |
| Guests | INT | Number of guests |
| TotalPrice | DECIMAL(18,2) | Total reservation price |
| Status | NVARCHAR(50) | Status: 'confirmed' or 'cancelled' |
| CreatedAt | DATETIME2 | Creation timestamp |

## Relationships

| Parent Table | Child Table | Relationship | On Delete |
|--------------|-------------|--------------|-----------|
| Owners | Properties | 1:N | RESTRICT |
| Properties | PropertyImages | 1:N | CASCADE |
| Properties | PropertyTraces | 1:N | CASCADE |
| Properties | Reservations | 1:N | CASCADE |

## Indexes

| Table | Index Name | Columns | Purpose |
|-------|------------|---------|---------|
| Properties | IX_Properties_IdOwner | IdOwner | Filter by owner |
| Properties | IX_Properties_Price | Price | Filter by price range |
| Properties | IX_Properties_Year | Year | Filter by year |
| PropertyImages | IX_PropertyImages_IdProperty | IdProperty | Get images by property |
| PropertyTraces | IX_PropertyTraces_IdProperty | IdProperty | Get traces by property |
| Reservations | IX_Reservations_IdProperty | IdProperty | Get reservations by property |
| Reservations | IX_Reservations_CheckIn_CheckOut | CheckIn, CheckOut | Check date conflicts |
