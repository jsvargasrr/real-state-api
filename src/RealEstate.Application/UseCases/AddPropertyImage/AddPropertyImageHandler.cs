using AutoMapper;
using FluentValidation;
using RealEstate.Application.Common;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.UseCases.AddPropertyImage;

/// <summary>
/// Handles the process of validating and adding an image to a property.
/// </summary>
public class AddPropertyImageHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<AddPropertyImageRequest> _validator;

    /// <summary>
    /// Creates a new instance of <see cref="AddPropertyImageHandler"/>.
    /// </summary>
    public AddPropertyImageHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<AddPropertyImageRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    /// <summary>
    /// Adds an image to a property after validating the request and ensuring the property exists.
    /// </summary>
    public async Task<Result<PropertyImageDto>> HandleAsync(Guid propertyId, AddPropertyImageRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<PropertyImageDto>.Failure(errors, "VALIDATION_ERROR");
        }

        var propertyExists = await _unitOfWork.Properties.ExistsAsync(propertyId, cancellationToken);
        if (!propertyExists)
        {
            return Result<PropertyImageDto>.Failure("Property not found", "NOT_FOUND");
        }

        var image = _mapper.Map<PropertyImage>(request);
        image.IdProperty = propertyId;

        await _unitOfWork.PropertyImages.AddAsync(image, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PropertyImageDto>(image);
        return Result<PropertyImageDto>.Success(dto);
    }
}
