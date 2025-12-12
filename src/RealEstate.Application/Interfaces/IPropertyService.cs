using RealEstate.Application.Common;
using RealEstate.Application.DTOs;

namespace RealEstate.Application.Interfaces;

public interface IPropertyService
{
    Task<Result<PropertyDto>> CreatePropertyAsync(CreatePropertyRequest request, CancellationToken cancellationToken = default);
    Task<Result<PropertyDto>> UpdatePropertyAsync(Guid id, UpdatePropertyRequest request, CancellationToken cancellationToken = default);
    Task<Result<PropertyDto>> ChangePriceAsync(Guid id, ChangePriceRequest request, CancellationToken cancellationToken = default);
    Task<Result<PropertyDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<PaginatedResult<PropertyDto>>> GetAllWithFiltersAsync(PropertyFilterRequest request, CancellationToken cancellationToken = default);
    Task<Result<PropertyImageDto>> AddImageAsync(Guid propertyId, AddPropertyImageRequest request, CancellationToken cancellationToken = default);
}
