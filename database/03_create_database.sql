-- =============================================
-- Real Estate Database - Create Database
-- Technical Test for Senior .NET Developer
-- =============================================
-- Execute this script FIRST to create the database
-- =============================================

-- Create Database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'RealEstateDB')
BEGIN
    CREATE DATABASE [RealEstateDB];
    PRINT 'Database RealEstateDB created successfully!';
END
ELSE
BEGIN
    PRINT 'Database RealEstateDB already exists.';
END
GO

USE [RealEstateDB]
GO

PRINT 'Database ready for table creation.';
GO
