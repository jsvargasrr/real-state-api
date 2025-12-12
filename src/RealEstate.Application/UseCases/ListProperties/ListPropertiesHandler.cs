using AutoMapper;
using RealEstate.Application.Common;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.UseCases.ListProperties;

/// <summary>
/// Retrieves a paginated and filtered list of properties.
/// </summary>
public class ListPropertiesHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates a new instance of <see cref="ListPropertiesHandler"/>.
    /// </summary>
    public ListPropertiesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Lists properties based on filter criteria and pagination settings.
    /// </summary>
    public async Task<Result<PaginatedResult<PropertyDto>>> HandleAsync(PropertyFilterRequest request, CancellationToken cancellationToken = default)
    {
        var (properties, totalCount) = await _unitOfWork.Properties.GetAllWithFiltersAsync(
            request.Name,
            request.Address,
            request.MinPrice,
            request.MaxPrice,
            request.Year,
            request.OwnerId,
            request.Page,
            request.PageSize,
            cancellationToken);

        var dtos = _mapper.Map<IEnumerable<PropertyDto>>(properties);
        var result = PaginatedResult<PropertyDto>.Create(dtos, totalCount, request.Page, request.PageSize);

        return Result<PaginatedResult<PropertyDto>>.Success(result);
    }
}
