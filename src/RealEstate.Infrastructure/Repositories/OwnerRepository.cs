using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

/// <summary>
/// Repository for managing owner entities using Entity Framework Core.
/// </summary>
public class OwnerRepository : IOwnerRepository
{
    private readonly RealEstateDbContext _context;

    /// <summary>
    /// Creates a new instance of <see cref="OwnerRepository"/>.
    /// </summary>
    public OwnerRepository(RealEstateDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets an owner by ID.
    /// </summary>
    public async Task<Owner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Owners
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.IdOwner == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all owners ordered by name.
    /// </summary>
    public async Task<IEnumerable<Owner>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Owners
            .AsNoTracking()
            .OrderBy(o => o.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a new owner to the database.
    /// </summary>
    public async Task<Owner> AddAsync(Owner owner, CancellationToken cancellationToken = default)
    {
        await _context.Owners.AddAsync(owner, cancellationToken);
        return owner;
    }

    /// <summary>
    /// Updates an existing owner.
    /// </summary>
    public Task UpdateAsync(Owner owner, CancellationToken cancellationToken = default)
    {
        _context.Owners.Update(owner);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Checks if an owner exists by ID.
    /// </summary>
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Owners.AnyAsync(o => o.IdOwner == id, cancellationToken);
    }
}
