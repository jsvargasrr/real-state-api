using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces;

public interface IPropertyTraceRepository
{
    Task<PropertyTrace?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(Guid propertyId, CancellationToken cancellationToken = default);
    Task<PropertyTrace> AddAsync(PropertyTrace propertyTrace, CancellationToken cancellationToken = default);
}
