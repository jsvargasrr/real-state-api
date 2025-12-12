-- =============================================
-- Real Estate Database - SQL Server Scripts
-- Technical Test for Senior .NET Developer
-- =============================================
-- Execute this script to create all tables
-- =============================================

USE [RealEstateDB]
GO

-- Create Owners table
CREATE TABLE [dbo].[Owners] (
    [IdOwner] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(200) NOT NULL,
    [Address] NVARCHAR(500) NOT NULL,
    [Photo] NVARCHAR(500) NULL,
    [Birthday] DATETIME2 NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL
);
GO

-- Create Properties table
CREATE TABLE [dbo].[Properties] (
    [IdProperty] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(200) NOT NULL,
    [Address] NVARCHAR(500) NOT NULL,
    [Price] DECIMAL(18, 2) NOT NULL,
    [CodeInternal] NVARCHAR(50) NOT NULL,
    [Year] INT NOT NULL,
    [IdOwner] UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    CONSTRAINT [FK_Properties_Owners] FOREIGN KEY ([IdOwner]) REFERENCES [dbo].[Owners]([IdOwner]),
    CONSTRAINT [UQ_Properties_CodeInternal] UNIQUE ([CodeInternal])
);
GO

-- Create PropertyImages table
CREATE TABLE [dbo].[PropertyImages] (
    [IdPropertyImage] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [IdProperty] UNIQUEIDENTIFIER NOT NULL,
    [File] NVARCHAR(500) NOT NULL,
    [Enabled] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_PropertyImages_Properties] FOREIGN KEY ([IdProperty]) REFERENCES [dbo].[Properties]([IdProperty]) ON DELETE CASCADE
);
GO

-- Create PropertyTraces table (Price History)
CREATE TABLE [dbo].[PropertyTraces] (
    [IdPropertyTrace] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [IdProperty] UNIQUEIDENTIFIER NOT NULL,
    [DateSale] DATETIME2 NOT NULL,
    [Name] NVARCHAR(200) NOT NULL,
    [Value] DECIMAL(18, 2) NOT NULL,
    [Tax] DECIMAL(18, 2) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_PropertyTraces_Properties] FOREIGN KEY ([IdProperty]) REFERENCES [dbo].[Properties]([IdProperty]) ON DELETE CASCADE
);
GO

-- Create Reservations table
CREATE TABLE [dbo].[Reservations] (
    [IdReservation] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [IdProperty] UNIQUEIDENTIFIER NOT NULL,
    [GuestName] NVARCHAR(200) NOT NULL,
    [GuestEmail] NVARCHAR(200) NOT NULL,
    [CheckIn] DATETIME2 NOT NULL,
    [CheckOut] DATETIME2 NOT NULL,
    [Guests] INT NOT NULL,
    [TotalPrice] DECIMAL(18, 2) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'confirmed',
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_Reservations_Properties] FOREIGN KEY ([IdProperty]) REFERENCES [dbo].[Properties]([IdProperty]) ON DELETE CASCADE
);
GO

-- Create indexes for better performance
CREATE INDEX [IX_Properties_IdOwner] ON [dbo].[Properties]([IdOwner]);
CREATE INDEX [IX_Properties_Price] ON [dbo].[Properties]([Price]);
CREATE INDEX [IX_Properties_Year] ON [dbo].[Properties]([Year]);
CREATE INDEX [IX_PropertyImages_IdProperty] ON [dbo].[PropertyImages]([IdProperty]);
CREATE INDEX [IX_PropertyTraces_IdProperty] ON [dbo].[PropertyTraces]([IdProperty]);
CREATE INDEX [IX_Reservations_IdProperty] ON [dbo].[Reservations]([IdProperty]);
CREATE INDEX [IX_Reservations_CheckIn_CheckOut] ON [dbo].[Reservations]([CheckIn], [CheckOut]);
GO

PRINT 'All tables created successfully!';
GO
