-- =============================================
-- Real Estate Database - Drop All Tables
-- Technical Test for Senior .NET Developer
-- =============================================
-- WARNING: This script will DELETE all data!
-- Use only for development/testing purposes
-- =============================================

USE [RealEstateDB]
GO

-- Drop tables in reverse order of dependencies
IF OBJECT_ID('dbo.Reservations', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reservations];

IF OBJECT_ID('dbo.PropertyTraces', 'U') IS NOT NULL
    DROP TABLE [dbo].[PropertyTraces];

IF OBJECT_ID('dbo.PropertyImages', 'U') IS NOT NULL
    DROP TABLE [dbo].[PropertyImages];

IF OBJECT_ID('dbo.Properties', 'U') IS NOT NULL
    DROP TABLE [dbo].[Properties];

IF OBJECT_ID('dbo.Owners', 'U') IS NOT NULL
    DROP TABLE [dbo].[Owners];

PRINT 'All tables dropped successfully!';
GO
