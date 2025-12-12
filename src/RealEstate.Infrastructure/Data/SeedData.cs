using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Data;

public static class SeedData
{
    public static async Task InitializeAsync(RealEstateDbContext context)
    {
        if (await context.Owners.AnyAsync())
            return;

        var owner1 = new Owner
        {
            IdOwner = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "María Fernanda López",
            Address = "Carrera 43A #1-50, El Poblado, Medellín",
            Photo = "https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=400",
            Birthday = DateTime.SpecifyKind(new DateTime(1978, 3, 15), DateTimeKind.Utc),
            CreatedAt = DateTime.UtcNow
        };

        var owner2 = new Owner
        {
            IdOwner = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Carlos Alberto Restrepo",
            Address = "Calle 10 #40-20, Laureles, Medellín",
            Photo = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400",
            Birthday = DateTime.SpecifyKind(new DateTime(1965, 8, 22), DateTimeKind.Utc),
            CreatedAt = DateTime.UtcNow
        };

        var owner3 = new Owner
        {
            IdOwner = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Name = "Ana María Gómez",
            Address = "Carrera 25 #2-15, Envigado, Antioquia",
            Photo = "https://images.unsplash.com/photo-1580489944761-15a19d654956?w=400",
            Birthday = DateTime.SpecifyKind(new DateTime(1982, 11, 8), DateTimeKind.Utc),
            CreatedAt = DateTime.UtcNow
        };

        var owner4 = new Owner
        {
            IdOwner = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Name = "Inversiones Antioquia S.A.S.",
            Address = "Calle 7 #42-30, El Poblado, Medellín",
            Photo = "https://images.unsplash.com/photo-1560179707-f14e90ef3623?w=400",
            Birthday = DateTime.SpecifyKind(new DateTime(2010, 1, 1), DateTimeKind.Utc),
            CreatedAt = DateTime.UtcNow
        };

        var owner5 = new Owner
        {
            IdOwner = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            Name = "Juan Pablo Mejía",
            Address = "Carrera 65 #48A-10, Estadio, Medellín",
            Photo = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=400",
            Birthday = DateTime.SpecifyKind(new DateTime(1975, 5, 20), DateTimeKind.Utc),
            CreatedAt = DateTime.UtcNow
        };

        await context.Owners.AddRangeAsync(owner1, owner2, owner3, owner4, owner5);

        var properties = new List<Property>
        {
            new()
            {
                IdProperty = Guid.Parse("aaaa1111-1111-1111-1111-111111111111"),
                Name = "Penthouse en El Poblado",
                Address = "Carrera 34 #7-20, El Poblado, Medellín",
                Price = 2850000000,
                CodeInternal = "PH-EPO-001",
                Year = 2022,
                IdOwner = owner1.IdOwner,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                IdProperty = Guid.Parse("aaaa2222-2222-2222-2222-222222222222"),
                Name = "Apartamento en Provenza",
                Address = "Calle 9 #43B-15, Provenza, Medellín",
                Price = 1450000000,
                CodeInternal = "AP-PRV-002",
                Year = 2021,
                IdOwner = owner1.IdOwner,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                IdProperty = Guid.Parse("aaaa3333-3333-3333-3333-333333333333"),
                Name = "Casa Campestre en Las Palmas",
                Address = "Kilómetro 8 Vía Las Palmas, Medellín",
                Price = 3200000000,
                CodeInternal = "CC-LPM-003",
                Year = 2019,
                IdOwner = owner2.IdOwner,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                IdProperty = Guid.Parse("aaaa4444-4444-4444-4444-444444444444"),
                Name = "Apartamento en Laureles",
                Address = "Circular 3 #70-45, Laureles, Medellín",
                Price = 680000000,
                CodeInternal = "AP-LAU-004",
                Year = 2018,
                IdOwner = owner2.IdOwner,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                IdProperty = Guid.Parse("aaaa5555-5555-5555-5555-555555555555"),
                Name = "Casa en Envigado",
                Address = "Carrera 43A #38S-25, Envigado, Antioquia",
                Price = 950000000,
                CodeInternal = "CA-ENV-005",
                Year = 2020,
                IdOwner = owner3.IdOwner,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                IdProperty = Guid.Parse("aaaa6666-6666-6666-6666-666666666666"),
                Name = "Apartamento en Sabaneta",
                Address = "Calle 77Sur #43A-10, Sabaneta, Antioquia",
                Price = 420000000,
                CodeInternal = "AP-SAB-006",
                Year = 2023,
                IdOwner = owner3.IdOwner,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                IdProperty = Guid.Parse("aaaa7777-7777-7777-7777-777777777777"),
                Name = "Oficina en Ciudad del Río",
                Address = "Carrera 48 #12-70, Ciudad del Río, Medellín",
                Price = 890000000,
                CodeInternal = "OF-CDR-007",
                Year = 2021,
                IdOwner = owner4.IdOwner,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                IdProperty = Guid.Parse("aaaa8888-8888-8888-8888-888888888888"),
                Name = "Local Comercial en El Tesoro",
                Address = "Centro Comercial El Tesoro, El Poblado, Medellín",
                Price = 1650000000,
                CodeInternal = "LC-TES-008",
                Year = 2020,
                IdOwner = owner4.IdOwner,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                IdProperty = Guid.Parse("aaaa9999-9999-9999-9999-999999999999"),
                Name = "Apartamento Estadio",
                Address = "Carrera 70 #44-15, Estadio, Medellín",
                Price = 520000000,
                CodeInternal = "AP-EST-009",
                Year = 2017,
                IdOwner = owner5.IdOwner,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                IdProperty = Guid.Parse("aaaabbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Name = "Loft en Manila",
                Address = "Calle 10 #38-30, Manila, Medellín",
                Price = 380000000,
                CodeInternal = "LF-MAN-010",
                Year = 2022,
                IdOwner = owner5.IdOwner,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Properties.AddRangeAsync(properties);

        var images = new List<PropertyImage>
        {
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[0].IdProperty, File = "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[0].IdProperty, File = "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[0].IdProperty, File = "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[1].IdProperty, File = "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[1].IdProperty, File = "https://images.unsplash.com/photo-1484154218962-a197022b5858?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[2].IdProperty, File = "https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[2].IdProperty, File = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[2].IdProperty, File = "https://images.unsplash.com/photo-1600566753086-00f18fb6b3ea?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[3].IdProperty, File = "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[3].IdProperty, File = "https://images.unsplash.com/photo-1560185127-6ed189bf02f4?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[4].IdProperty, File = "https://images.unsplash.com/photo-1583608205776-bfd35f0d9f83?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[4].IdProperty, File = "https://images.unsplash.com/photo-1600047509807-ba8f99d2cdde?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[5].IdProperty, File = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[5].IdProperty, File = "https://images.unsplash.com/photo-1493809842364-78817add7ffb?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[6].IdProperty, File = "https://images.unsplash.com/photo-1497366216548-37526070297c?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[6].IdProperty, File = "https://images.unsplash.com/photo-1497366754035-f200968a6e72?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[7].IdProperty, File = "https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[7].IdProperty, File = "https://images.unsplash.com/photo-1604719312566-8912e9227c6a?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[8].IdProperty, File = "https://images.unsplash.com/photo-1502672023488-70e25813eb80?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[8].IdProperty, File = "https://images.unsplash.com/photo-1560185007-c5ca9d2c014d?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[9].IdProperty, File = "https://images.unsplash.com/photo-1536376072261-38c75010e6c9?w=800", Enabled = true, CreatedAt = DateTime.UtcNow },
            new() { IdPropertyImage = Guid.NewGuid(), IdProperty = properties[9].IdProperty, File = "https://images.unsplash.com/photo-1554995207-c18c203602cb?w=800", Enabled = true, CreatedAt = DateTime.UtcNow }
        };

        await context.PropertyImages.AddRangeAsync(images);

        var traces = new List<PropertyTrace>
        {
            new() { IdPropertyTrace = Guid.NewGuid(), IdProperty = properties[0].IdProperty, Name = "Precio inicial", Value = 2500000000, Tax = 475000000, DateSale = DateTime.SpecifyKind(new DateTime(2022, 6, 15), DateTimeKind.Utc), CreatedAt = DateTime.UtcNow },
            new() { IdPropertyTrace = Guid.NewGuid(), IdProperty = properties[0].IdProperty, Name = "Ajuste por valorización", Value = 2850000000, Tax = 541500000, DateSale = DateTime.SpecifyKind(new DateTime(2024, 1, 10), DateTimeKind.Utc), CreatedAt = DateTime.UtcNow },
            new() { IdPropertyTrace = Guid.NewGuid(), IdProperty = properties[2].IdProperty, Name = "Precio inicial", Value = 2800000000, Tax = 532000000, DateSale = DateTime.SpecifyKind(new DateTime(2019, 3, 20), DateTimeKind.Utc), CreatedAt = DateTime.UtcNow },
            new() { IdPropertyTrace = Guid.NewGuid(), IdProperty = properties[2].IdProperty, Name = "Remodelación cocina", Value = 3200000000, Tax = 608000000, DateSale = DateTime.SpecifyKind(new DateTime(2023, 8, 15), DateTimeKind.Utc), CreatedAt = DateTime.UtcNow },
            new() { IdPropertyTrace = Guid.NewGuid(), IdProperty = properties[3].IdProperty, Name = "Precio inicial", Value = 580000000, Tax = 110200000, DateSale = DateTime.SpecifyKind(new DateTime(2018, 11, 1), DateTimeKind.Utc), CreatedAt = DateTime.UtcNow },
            new() { IdPropertyTrace = Guid.NewGuid(), IdProperty = properties[3].IdProperty, Name = "Valorización zona", Value = 680000000, Tax = 129200000, DateSale = DateTime.SpecifyKind(new DateTime(2023, 5, 20), DateTimeKind.Utc), CreatedAt = DateTime.UtcNow }
        };

        await context.PropertyTraces.AddRangeAsync(traces);

        var reservations = new List<Reservation>
        {
            new() { IdReservation = Guid.NewGuid(), IdProperty = properties[1].IdProperty, GuestName = "Roberto García", GuestEmail = "roberto.garcia@email.com", CheckIn = DateTime.SpecifyKind(new DateTime(2025, 1, 15), DateTimeKind.Utc), CheckOut = DateTime.SpecifyKind(new DateTime(2025, 1, 20), DateTimeKind.Utc), Guests = 2, TotalPrice = 2500000, Status = "Confirmada", CreatedAt = DateTime.UtcNow },
            new() { IdReservation = Guid.NewGuid(), IdProperty = properties[3].IdProperty, GuestName = "Laura Martínez", GuestEmail = "laura.martinez@email.com", CheckIn = DateTime.SpecifyKind(new DateTime(2025, 2, 1), DateTimeKind.Utc), CheckOut = DateTime.SpecifyKind(new DateTime(2025, 2, 7), DateTimeKind.Utc), Guests = 3, TotalPrice = 4200000, Status = "Pendiente", CreatedAt = DateTime.UtcNow },
            new() { IdReservation = Guid.NewGuid(), IdProperty = properties[5].IdProperty, GuestName = "Andrés Sánchez", GuestEmail = "andres.sanchez@email.com", CheckIn = DateTime.SpecifyKind(new DateTime(2025, 1, 25), DateTimeKind.Utc), CheckOut = DateTime.SpecifyKind(new DateTime(2025, 1, 28), DateTimeKind.Utc), Guests = 1, TotalPrice = 1200000, Status = "Confirmada", CreatedAt = DateTime.UtcNow }
        };

        await context.Reservations.AddRangeAsync(reservations);
        await context.SaveChangesAsync();
    }
}
