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
public class GetPropertyReservationsHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private GetPropertyReservationsHandler _handler;
    private Property _testProperty;
    private Guid _propertyId;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPropertyReservationsHandler(_unitOfWorkMock.Object, _mapperMock.Object);

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
    public async Task HandleAsync_WithNonExistentProperty_ShouldReturnNotFound()
    {
        _unitOfWorkMock.Setup(x => x.Properties.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property?)null);

        var result = await _handler.HandleAsync(_propertyId, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("NOT_FOUND");
    }

    [Test]
    public async Task HandleAsync_WithValidProperty_ShouldReturnReservations()
    {
        var reservations = new List<Reservation>
        {
            new Reservation
            {
                IdReservation = Guid.NewGuid(),
                IdProperty = _propertyId,
                GuestName = "John Doe",
                GuestEmail = "john@example.com",
                CheckIn = DateTime.Today.AddDays(1),
                CheckOut = DateTime.Today.AddDays(3),
                Guests = 2,
                TotalPrice = 200000,
                Status = "confirmed"
            }
        };

        var expectedDtos = new List<ReservationDto>
        {
            new ReservationDto(
                reservations[0].IdReservation,
                _propertyId,
                "John Doe",
                "john@example.com",
                reservations[0].CheckIn,
                reservations[0].CheckOut,
                2,
                200000,
                "confirmed",
                DateTime.UtcNow
            )
        };

        _unitOfWorkMock.Setup(x => x.Properties.GetByIdAsync(_propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_testProperty);
        _unitOfWorkMock.Setup(x => x.Reservations.GetByPropertyIdAsync(_propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(reservations);
        _mapperMock.Setup(x => x.Map<IEnumerable<ReservationDto>>(reservations))
            .Returns(expectedDtos);

        var result = await _handler.HandleAsync(_propertyId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(1);
    }

    [Test]
    public async Task HandleAsync_WithNoReservations_ShouldReturnEmptyList()
    {
        var emptyReservations = new List<Reservation>();
        var emptyDtos = new List<ReservationDto>();

        _unitOfWorkMock.Setup(x => x.Properties.GetByIdAsync(_propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_testProperty);
        _unitOfWorkMock.Setup(x => x.Reservations.GetByPropertyIdAsync(_propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyReservations);
        _mapperMock.Setup(x => x.Map<IEnumerable<ReservationDto>>(emptyReservations))
            .Returns(emptyDtos);

        var result = await _handler.HandleAsync(_propertyId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }
}
