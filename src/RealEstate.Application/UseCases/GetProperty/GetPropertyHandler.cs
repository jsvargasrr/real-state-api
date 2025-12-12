using AutoMapper;
using RealEstate.Application.Common;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.UseCases.GetProperty;

/// <summary>
/// Retrieves a property along with its related images.
/// </summary>
public class GetPropertyHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates a new instance of <see cref="GetPropertyHandler"/>.
    /// </summary>
    public GetPropertyHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a property by ID, returning its details and associated images.
    /// </summary>
    public async Task<Result<PropertyDto>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var property = await _unitOfWork.Properties.GetByIdWithImagesAsync(id, cancellationToken);

        if (property == null)
        {
            return Result<PropertyDto>.Failure("Property not found", "NOT_FOUND");
        }

        var dto = _mapper.Map<PropertyDto>(property);
        return Result<PropertyDto>.Success(dto);
    }
}
