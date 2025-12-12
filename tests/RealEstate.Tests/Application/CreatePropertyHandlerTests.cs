using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using RealEstate.Application.DTOs;
using RealEstate.Application.Mappings;
using RealEstate.Application.UseCases.CreateProperty;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Tests.Application;

[TestFixture]
public class CreatePropertyHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IValidator<CreatePropertyRequest>> _validatorMock;
    private IMapper _mapper;
    private CreatePropertyHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<CreatePropertyRequest>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _handler = new CreatePropertyHandler(_unitOfWorkMock.Object, _mapper, _validatorMock.Object);
    }

    [Test]
    public async Task HandleAsync_WithValidRequest_ShouldReturnSuccess()
    {
        var ownerId = Guid.NewGuid();
        var request = new CreatePropertyRequest(
            Name: "Beautiful House",
            Address: "123 Main St, New York, NY",
            Price: 500000,
            CodeInternal: "PROP-001",
            Year: 2020,
            IdOwner: ownerId
        );

        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _unitOfWorkMock
            .Setup(u => u.Owners.ExistsAsync(ownerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(u => u.Properties.CodeInternalExistsAsync(request.CodeInternal, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _unitOfWorkMock
            .Setup(u => u.Properties.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property p, CancellationToken _) => p);

        _unitOfWorkMock
            .Setup(u => u.Properties.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid id, CancellationToken _) => new Property
            {
                IdProperty = id,
                Name = request.Name,
                Address = request.Address,
                Price = request.Price,
                CodeInternal = request.CodeInternal,
                Year = request.Year,
                IdOwner = ownerId,
                Owner = new Owner { IdOwner = ownerId, Name = "Test Owner", Address = "Test", Birthday = DateTime.Now }
            });

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.HandleAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be(request.Name);
        result.Data.Price.Should().Be(request.Price);
    }

    [Test]
    public async Task HandleAsync_WithInvalidRequest_ShouldReturnValidationError()
    {
        var request = new CreatePropertyRequest(
            Name: "",
            Address: "123 Main St",
            Price: -100,
            CodeInternal: "PROP-001",
            Year: 2020,
            IdOwner: Guid.NewGuid()
        );

        var validationFailures = new List<ValidationFailure>
        {
            new("Name", "Name is required"),
            new("Price", "Price must be greater than 0")
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var result = await _handler.HandleAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("VALIDATION_ERROR");
        result.Error.Should().Contain("Name is required");
    }

    [Test]
    public async Task HandleAsync_WithNonExistentOwner_ShouldReturnOwnerNotFound()
    {
        var request = new CreatePropertyRequest(
            Name: "Test Property",
            Address: "123 Test St",
            Price: 100000,
            CodeInternal: "PROP-002",
            Year: 2021,
            IdOwner: Guid.NewGuid()
        );

        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _unitOfWorkMock
            .Setup(u => u.Owners.ExistsAsync(request.IdOwner, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _handler.HandleAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("OWNER_NOT_FOUND");
    }

    [Test]
    public async Task HandleAsync_WithDuplicateCodeInternal_ShouldReturnDuplicateError()
    {
        var ownerId = Guid.NewGuid();
        var request = new CreatePropertyRequest(
            Name: "Test Property",
            Address: "123 Test St",
            Price: 100000,
            CodeInternal: "EXISTING-CODE",
            Year: 2021,
            IdOwner: ownerId
        );

        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _unitOfWorkMock
            .Setup(u => u.Owners.ExistsAsync(ownerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(u => u.Properties.CodeInternalExistsAsync(request.CodeInternal, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _handler.HandleAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("DUPLICATE_CODE");
    }
}
