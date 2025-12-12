using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces;

public interface IPropertyImageRepository
{
    Task<PropertyImage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(Guid propertyId, CancellationToken cancellationToken = default);
    Task<PropertyImage> AddAsync(PropertyImage propertyImage, CancellationToken cancellationToken = default);
    Task UpdateAsync(PropertyImage propertyImage, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
