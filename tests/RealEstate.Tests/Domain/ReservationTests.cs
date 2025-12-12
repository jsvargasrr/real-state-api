using FluentAssertions;
using NUnit.Framework;
using RealEstate.Domain.Entities;

namespace RealEstate.Tests.Domain;

[TestFixture]
public class ReservationTests
{
    [Test]
    public void Reservation_WhenCreated_ShouldHaveDefaultStatus()
    {
        var reservation = new Reservation();

        reservation.Status.Should().Be("confirmed");
    }

    [Test]
    public void Reservation_WhenCreated_ShouldHaveCreatedAtSet()
    {
        var before = DateTime.UtcNow.AddSeconds(-1);
        var reservation = new Reservation();
        var after = DateTime.UtcNow.AddSeconds(1);

        reservation.CreatedAt.Should().BeAfter(before);
        reservation.CreatedAt.Should().BeBefore(after);
    }

    [Test]
    public void Reservation_ShouldCalculateNightsCorrectly()
    {
        var reservation = new Reservation
        {
            CheckIn = new DateTime(2025, 12, 15),
            CheckOut = new DateTime(2025, 12, 18)
        };

        var nights = (reservation.CheckOut - reservation.CheckIn).Days;

        nights.Should().Be(3);
    }

    [Test]
    public void Reservation_ShouldStoreAllProperties()
    {
        var propertyId = Guid.NewGuid();
        var reservationId = Guid.NewGuid();
        
        var reservation = new Reservation
        {
            IdReservation = reservationId,
            IdProperty = propertyId,
            GuestName = "Juan Pérez",
            GuestEmail = "juan@email.com",
            CheckIn = new DateTime(2025, 12, 20),
            CheckOut = new DateTime(2025, 12, 25),
            Guests = 4,
            TotalPrice = 833333,
            Status = "confirmed"
        };

        reservation.IdReservation.Should().Be(reservationId);
        reservation.IdProperty.Should().Be(propertyId);
        reservation.GuestName.Should().Be("Juan Pérez");
        reservation.GuestEmail.Should().Be("juan@email.com");
        reservation.Guests.Should().Be(4);
        reservation.TotalPrice.Should().Be(833333);
        reservation.Status.Should().Be("confirmed");
    }

    [Test]
    public void Reservation_StatusCanBeChangedToCancelled()
    {
        var reservation = new Reservation
        {
            Status = "confirmed"
        };

        reservation.Status = "cancelled";

        reservation.Status.Should().Be("cancelled");
    }
}
