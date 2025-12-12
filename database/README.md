# Real Estate Database Scripts

## Overview

This folder contains all the SQL Server scripts needed to set up the Real Estate database for production deployment.

## Files

| File | Description | Order |
|------|-------------|-------|
| `03_create_database.sql` | Creates the RealEstateDB database | 1 |
| `01_create_tables.sql` | Creates all tables with relationships and indexes | 2 |
| `02_seed_data.sql` | Inserts sample data for testing | 3 |
| `04_drop_all.sql` | Drops all tables (use with caution!) | - |
| `DATABASE_DIAGRAM.md` | Entity relationship diagram and documentation | - |

## Setup Instructions

### Option 1: Using SQL Server Management Studio (SSMS)

1. Open SSMS and connect to your SQL Server instance
2. Execute scripts in this order:
   - `03_create_database.sql`
   - `01_create_tables.sql`
   - `02_seed_data.sql` (optional, for sample data)

### Option 2: Using sqlcmd

```bash
sqlcmd -S your_server -U your_user -P your_password -i 03_create_database.sql
sqlcmd -S your_server -U your_user -P your_password -d RealEstateDB -i 01_create_tables.sql
sqlcmd -S your_server -U your_user -P your_password -d RealEstateDB -i 02_seed_data.sql
```

### Option 3: Using Entity Framework Migrations

The application also supports EF Core migrations. Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=RealEstateDB;User Id=your_user;Password=your_password;TrustServerCertificate=True;"
  }
}
```

Then run:
```bash
cd src/RealEstate.Api
dotnet ef database update
```

## Connection String Format

### SQL Server
```
Server=localhost;Database=RealEstateDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;
```

### PostgreSQL (Development)
```
Host=localhost;Database=realestate;Username=postgres;Password=postgres;
```

## Database Backup

To create a backup (.bak file) in SQL Server:

```sql
BACKUP DATABASE [RealEstateDB] 
TO DISK = 'C:\Backups\RealEstateDB.bak'
WITH FORMAT, INIT, NAME = 'RealEstateDB Full Backup';
```

To restore from backup:

```sql
RESTORE DATABASE [RealEstateDB]
FROM DISK = 'C:\Backups\RealEstateDB.bak'
WITH RECOVERY;
```

## Notes

- All prices are in **Colombian Pesos (COP)**
- UUIDs are used for all primary keys for better distribution
- PropertyTraces table maintains price change history for auditing
- Reservations support conflict detection (no overlapping dates per property)
