using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RealEstate.Application.DTOs;
using RealEstate.Application.UseCases.Reservations;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Tests.Application;

[TestFixture]
public class CreateReservationHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private CreateReservationHandler _handler;
    private Property _testProperty;
    private Guid _propertyId;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateReservationHandler(_unitOfWorkMock.Object, _mapperMock.Object);

        _propertyId = Guid.NewGuid();
        _testProperty = new Property
        {
            IdProperty = _propertyId,
            Name = "Test Property",
            Address = "Test Address",
            Price = 3000000,
            CodeInternal = "TEST-001",
            Year = 2023,
            IdOwner = Guid.NewGuid()
        };
    }

    [Test]
    public async Task HandleAsync_WithEmptyGuestName_ShouldReturnValidationError()
    {
        var request = new CreateReservationRequest("", "guest@email.com", DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 2);

        var result = await _handler.HandleAsync(_propertyId, request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("VALIDATION_ERROR");
        result.Error.Should().Contain("Guest name");
    }

    [Test]
    public async Task HandleAsync_WithEmptyGuestEmail_ShouldReturnValidationError()
    {
        var request = new CreateReservationRequest("John Doe", "", DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 2);

        var result = await _handler.HandleAsync(_propertyId, request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("VALIDATION_ERROR");
        result.Error.Should().Contain("Guest email");
    }

    [Test]
    public async Task HandleAsync_WithCheckOutBeforeCheckIn_ShouldReturnValidationError()
    {
        var request = new CreateReservationRequest("John Doe", "guest@email.com", DateTime.Today.AddDays(5), DateTime.Today.AddDays(2), 2);

        var result = await _handler.HandleAsync(_propertyId, request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("VALIDATION_ERROR");
        result.Error.Should().Contain("Check-out must be after check-in");
    }

    [Test]
    public async Task HandleAsync_WithCheckInInPast_ShouldReturnValidationError()
    {
        var request = new CreateReservationRequest("John Doe", "guest@email.com", DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2), 2);

        var result = await _handler.HandleAsync(_propertyId, request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("VALIDATION_ERROR");
        result.Error.Should().Contain("past");
    }

    [Test]
    public async Task HandleAsync_WithZeroGuests_ShouldReturnValidationError()
    {
        var request = new CreateReservationRequest("John Doe", "guest@email.com", DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 0);

        var result = await _handler.HandleAsync(_propertyId, request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("VALIDATION_ERROR");
        result.Error.Should().Contain("guest");
    }

    [Test]
    public async Task HandleAsync_WithNonExistentProperty_ShouldReturnNotFound()
    {
        var request = new CreateReservationRequest("John Doe", "guest@email.com", DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 2);
        _unitOfWorkMock.Setup(x => x.Properties.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property?)null);

        var result = await _handler.HandleAsync(_propertyId, request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("NOT_FOUND");
    }

    [Test]
    public async Task HandleAsync_WithDateConflict_ShouldReturnConflictError()
    {
        var request = new CreateReservationRequest("John Doe", "guest@email.com", DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 2);
        _unitOfWorkMock.Setup(x => x.Properties.GetByIdAsync(_propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_testProperty);
        _unitOfWorkMock.Setup(x => x.Reservations.HasConflictAsync(_propertyId, It.IsAny<DateTime>(), It.IsAny<DateTime>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _handler.HandleAsync(_propertyId, request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("DATE_CONFLICT");
    }

    [Test]
    public async Task HandleAsync_WithValidRequest_ShouldCreateReservation()
    {
        var request = new CreateReservationRequest("John Doe", "guest@email.com", DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 2);
        var expectedDto = new ReservationDto(
            Guid.NewGuid(),
            _propertyId,
            "John Doe",
            "guest@email.com",
            request.CheckIn,
            request.CheckOut,
            2,
            200000,
            "confirmed",
            DateTime.UtcNow
        );

        _unitOfWorkMock.Setup(x => x.Properties.GetByIdAsync(_propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_testProperty);
        _unitOfWorkMock.Setup(x => x.Reservations.HasConflictAsync(_propertyId, It.IsAny<DateTime>(), It.IsAny<DateTime>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _unitOfWorkMock.Setup(x => x.Reservations.AddAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Reservation r, CancellationToken _) => r);
        _mapperMock.Setup(x => x.Map<ReservationDto>(It.IsAny<Reservation>()))
            .Returns(expectedDto);

        var result = await _handler.HandleAsync(_propertyId, request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.GuestName.Should().Be("John Doe");
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
