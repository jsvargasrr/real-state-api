using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using RealEstate.Application.DTOs;
using RealEstate.Application.Mappings;
using RealEstate.Application.UseCases.ChangePropertyPrice;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Tests.Application;

[TestFixture]
public class ChangePropertyPriceHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IValidator<ChangePriceRequest>> _validatorMock;
    private IMapper _mapper;
    private ChangePropertyPriceHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<ChangePriceRequest>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _handler = new ChangePropertyPriceHandler(_unitOfWorkMock.Object, _mapper, _validatorMock.Object);
    }

    [Test]
    public async Task HandleAsync_WithValidRequest_ShouldChangePriceAndCreateTrace()
    {
        var propertyId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var oldPrice = 300000m;
        var newPrice = 350000m;
        var request = new ChangePriceRequest(NewPrice: newPrice);

        var property = new Property
        {
            IdProperty = propertyId,
            Name = "Test Property",
            Address = "123 Test St",
            Price = oldPrice,
            CodeInternal = "TEST-001",
            Year = 2020,
            IdOwner = ownerId,
            Owner = new Owner { IdOwner = ownerId, Name = "Test Owner", Address = "Test", Birthday = DateTime.Now }
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _unitOfWorkMock
            .Setup(u => u.Properties.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(property);

        _unitOfWorkMock
            .Setup(u => u.Properties.UpdateAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.HandleAsync(propertyId, request);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Price.Should().Be(newPrice);
        property.PropertyTraces.Should().HaveCount(1);
    }

    [Test]
    public async Task HandleAsync_WithNonExistentProperty_ShouldReturnNotFound()
    {
        var propertyId = Guid.NewGuid();
        var request = new ChangePriceRequest(NewPrice: 500000);

        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _unitOfWorkMock
            .Setup(u => u.Properties.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property?)null);

        var result = await _handler.HandleAsync(propertyId, request);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("NOT_FOUND");
    }

    [Test]
    public async Task HandleAsync_WithNegativePrice_ShouldReturnValidationError()
    {
        var propertyId = Guid.NewGuid();
        var request = new ChangePriceRequest(NewPrice: -100);

        var validationFailures = new List<ValidationFailure>
        {
            new("NewPrice", "Price must be greater than 0")
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var result = await _handler.HandleAsync(propertyId, request);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("VALIDATION_ERROR");
    }
}
