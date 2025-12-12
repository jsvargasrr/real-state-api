using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Property?> GetByIdWithImagesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Property> Properties, int TotalCount)> GetAllWithFiltersAsync(
        string? name = null,
        string? address = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int? year = null,
        Guid? ownerId = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
    Task<Property> AddAsync(Property property, CancellationToken cancellationToken = default);
    Task UpdateAsync(Property property, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> CodeInternalExistsAsync(string codeInternal, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
