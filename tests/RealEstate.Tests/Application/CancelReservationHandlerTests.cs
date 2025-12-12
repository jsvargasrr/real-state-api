using FluentAssertions;
using Moq;
using NUnit.Framework;
using RealEstate.Application.UseCases.Reservations;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Tests.Application;

[TestFixture]
public class CancelReservationHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private CancelReservationHandler _handler;
    private Guid _reservationId;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CancelReservationHandler(_unitOfWorkMock.Object);
        _reservationId = Guid.NewGuid();
    }

    [Test]
    public async Task HandleAsync_WithNonExistentReservation_ShouldReturnNotFound()
    {
        _unitOfWorkMock.Setup(x => x.Reservations.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Reservation?)null);

        var result = await _handler.HandleAsync(_reservationId, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("NOT_FOUND");
        result.Error.Should().Contain("not found");
    }

    [Test]
    public async Task HandleAsync_WithValidReservation_ShouldCancelSuccessfully()
    {
        var reservation = new Reservation
        {
            IdReservation = _reservationId,
            IdProperty = Guid.NewGuid(),
            GuestName = "John Doe",
            GuestEmail = "john@example.com",
            CheckIn = DateTime.Today.AddDays(1),
            CheckOut = DateTime.Today.AddDays(3),
            Guests = 2,
            TotalPrice = 200000,
            Status = "confirmed"
        };

        _unitOfWorkMock.Setup(x => x.Reservations.GetByIdAsync(_reservationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(reservation);

        var result = await _handler.HandleAsync(_reservationId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeTrue();
        reservation.Status.Should().Be("cancelled");
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
