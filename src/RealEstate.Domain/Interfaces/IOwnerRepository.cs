using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces;

public interface IOwnerRepository
{
    Task<Owner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Owner>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Owner> AddAsync(Owner owner, CancellationToken cancellationToken = default);
    Task UpdateAsync(Owner owner, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
