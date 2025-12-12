-- =============================================
-- Real Estate Database - Seed Data
-- Technical Test for Senior .NET Developer
-- =============================================
-- Execute this script to insert sample data
-- =============================================

USE [RealEstateDB]
GO

-- Insert Owners
INSERT INTO [dbo].[Owners] ([IdOwner], [Name], [Address], [Photo], [Birthday], [CreatedAt])
VALUES 
    ('11111111-1111-1111-1111-111111111111', 'Robert Johnson', '123 Park Avenue, New York, NY 10001', 'https://images.unsplash.com/photo-1560250097-0b93528c311a?w=200', '1975-05-15', GETUTCDATE()),
    ('22222222-2222-2222-2222-222222222222', 'Maria Garcia', '456 Ocean Drive, Miami, FL 33139', 'https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=200', '1982-08-22', GETUTCDATE()),
    ('33333333-3333-3333-3333-333333333333', 'James Smith', '789 Mountain Road, Denver, CO 80202', 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=200', '1968-12-10', GETUTCDATE());
GO

-- Insert Properties (Prices in Colombian Pesos - COP)
INSERT INTO [dbo].[Properties] ([IdProperty], [Name], [Address], [Price], [CodeInternal], [Year], [IdOwner], [CreatedAt])
VALUES 
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Luxury Penthouse Manhattan', '100 Central Park West, New York, NY 10023', 5000000, 'NYC-PENT-001', 2021, '11111111-1111-1111-1111-111111111111', GETUTCDATE()),
    ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Beachfront Villa Miami', '200 Collins Avenue, Miami Beach, FL 33140', 5000000, 'MIA-VILLA-001', 2020, '22222222-2222-2222-2222-222222222222', GETUTCDATE()),
    ('cccccccc-cccc-cccc-cccc-cccccccccccc', 'Mountain Retreat Aspen', '300 Aspen Mountain Road, Aspen, CO 81611', 2000000, 'ASP-RET-001', 2019, '33333333-3333-3333-3333-333333333333', GETUTCDATE()),
    ('dddddddd-dddd-dddd-dddd-dddddddddddd', 'Modern Loft Downtown LA', '400 Broadway, Los Angeles, CA 90013', 2000000, 'LA-LOFT-001', 2022, '11111111-1111-1111-1111-111111111111', GETUTCDATE()),
    ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 'Historic Brownstone Boston', '500 Beacon Street, Boston, MA 02116', 1000000, 'BOS-BROWN-001', 2018, '22222222-2222-2222-2222-222222222222', GETUTCDATE()),
    ('ffffffff-ffff-ffff-ffff-ffffffffffff', 'Waterfront Condo Seattle', '600 Pike Street, Seattle, WA 98101', 1000000, 'SEA-CONDO-001', 2023, '33333333-3333-3333-3333-333333333333', GETUTCDATE());
GO

-- Insert Property Images
INSERT INTO [dbo].[PropertyImages] ([IdProperty], [File], [Enabled], [CreatedAt])
VALUES 
    -- Manhattan Penthouse
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800&q=80', 1, GETUTCDATE()),
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800&q=80', 1, GETUTCDATE()),
    -- Miami Villa
    ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800&q=80', 1, GETUTCDATE()),
    ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=800&q=80', 1, GETUTCDATE()),
    -- Mountain Retreat
    ('cccccccc-cccc-cccc-cccc-cccccccccccc', 'https://images.unsplash.com/photo-1518780664697-55e3ad937233?w=800&q=80', 1, GETUTCDATE()),
    -- LA Loft
    ('dddddddd-dddd-dddd-dddd-dddddddddddd', 'https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800&q=80', 1, GETUTCDATE()),
    -- Boston Brownstone
    ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 'https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=800&q=80', 1, GETUTCDATE()),
    -- Seattle Condo
    ('ffffffff-ffff-ffff-ffff-ffffffffffff', 'https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?w=800&q=80', 1, GETUTCDATE());
GO

PRINT 'Sample data inserted successfully!';
GO
