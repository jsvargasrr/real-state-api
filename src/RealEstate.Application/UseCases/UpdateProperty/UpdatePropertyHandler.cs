using AutoMapper;
using FluentValidation;
using RealEstate.Application.Common;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.UseCases.UpdateProperty;

/// <summary>
/// Handles updating a property, including validation, owner verification, and unique code checks.
/// </summary>
public class UpdatePropertyHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdatePropertyRequest> _validator;

    /// <summary>
    /// Creates a new instance of <see cref="UpdatePropertyHandler"/>.
    /// </summary>
    public UpdatePropertyHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdatePropertyRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    /// <summary>
    /// Updates a property after validating input, checking owner existence, and ensuring internal code uniqueness.
    /// </summary>
    public async Task<Result<PropertyDto>> HandleAsync(Guid id, UpdatePropertyRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<PropertyDto>.Failure(errors, "VALIDATION_ERROR");
        }

        var property = await _unitOfWork.Properties.GetByIdAsync(id, cancellationToken);
        if (property == null)
        {
            return Result<PropertyDto>.Failure("Property not found", "NOT_FOUND");
        }

        var ownerExists = await _unitOfWork.Owners.ExistsAsync(request.IdOwner, cancellationToken);
        if (!ownerExists)
        {
            return Result<PropertyDto>.Failure("Owner not found", "OWNER_NOT_FOUND");
        }

        var codeExists = await _unitOfWork.Properties.CodeInternalExistsAsync(request.CodeInternal, id, cancellationToken);
        if (codeExists)
        {
            return Result<PropertyDto>.Failure("CodeInternal already exists", "DUPLICATE_CODE");
        }

        property.Name = request.Name;
        property.Address = request.Address;
        property.CodeInternal = request.CodeInternal;
        property.Year = request.Year;
        property.IdOwner = request.IdOwner;
        property.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Properties.UpdateAsync(property, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedProperty = await _unitOfWork.Properties.GetByIdAsync(id, cancellationToken);
        var dto = _mapper.Map<PropertyDto>(updatedProperty);

        return Result<PropertyDto>.Success(dto);
    }
}
