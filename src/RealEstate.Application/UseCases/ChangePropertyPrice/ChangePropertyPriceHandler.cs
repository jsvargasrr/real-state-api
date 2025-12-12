using AutoMapper;
using FluentValidation;
using RealEstate.Application.Common;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.UseCases.ChangePropertyPrice;

/// <summary>
/// Handles the process of validating and updating the price of a property.
/// </summary>
public class ChangePropertyPriceHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<ChangePriceRequest> _validator;

    /// <summary>
    /// Creates a new instance of <see cref="ChangePropertyPriceHandler"/>.
    /// </summary>
    public ChangePropertyPriceHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<ChangePriceRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    /// <summary>
    /// Updates the price of a property after validating the request and ensuring the property exists.
    /// </summary>
    public async Task<Result<PropertyDto>> HandleAsync(Guid id, ChangePriceRequest request, CancellationToken cancellationToken = default)
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

        property.ChangePrice(request.NewPrice);

        await _unitOfWork.Properties.UpdateAsync(property, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedProperty = await _unitOfWork.Properties.GetByIdAsync(id, cancellationToken);
        var dto = _mapper.Map<PropertyDto>(updatedProperty);

        return Result<PropertyDto>.Success(dto);
    }
}
