using AutoMapper;
using FluentValidation;
using RealEstate.Application.Common;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.UseCases.CreateProperty;

/// <summary>
/// Handles the creation of a property, ensuring validation, owner existence, and unique internal code.
/// </summary>
public class CreatePropertyHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreatePropertyRequest> _validator;

    /// <summary>
    /// Creates a new instance of <see cref="CreatePropertyHandler"/>.
    /// </summary>
    public CreatePropertyHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreatePropertyRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    /// <summary>
    /// Creates a property after validating input, checking owner existence, and ensuring the internal code is unique.
    /// </summary>
    public async Task<Result<PropertyDto>> HandleAsync(CreatePropertyRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<PropertyDto>.Failure(errors, "VALIDATION_ERROR");
        }

        var ownerExists = await _unitOfWork.Owners.ExistsAsync(request.IdOwner, cancellationToken);
        if (!ownerExists)
        {
            return Result<PropertyDto>.Failure("Owner not found", "OWNER_NOT_FOUND");
        }

        var codeExists = await _unitOfWork.Properties.CodeInternalExistsAsync(request.CodeInternal, null, cancellationToken);
        if (codeExists)
        {
            return Result<PropertyDto>.Failure("CodeInternal already exists", "DUPLICATE_CODE");
        }

        var property = _mapper.Map<Property>(request);
        await _unitOfWork.Properties.AddAsync(property, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var createdProperty = await _unitOfWork.Properties.GetByIdAsync(property.IdProperty, cancellationToken);
        var dto = _mapper.Map<PropertyDto>(createdProperty);

        return Result<PropertyDto>.Success(dto);
    }
}
