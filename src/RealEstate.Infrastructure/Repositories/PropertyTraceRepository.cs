using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

/// <summary>
/// Repository for managing property trace records (historical sales or changes).
/// </summary>
public class PropertyTraceRepository : IPropertyTraceRepository
{
    private readonly RealEstateDbContext _context;

    /// <summary>
    /// Creates a new instance of <see cref="PropertyTraceRepository"/>.
    /// </summary>
    public PropertyTraceRepository(RealEstateDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a property trace by its ID.
    /// </summary>
    public async Task<PropertyTrace?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.PropertyTraces
            .AsNoTracking()
            .FirstOrDefaultAsync(pt => pt.IdPropertyTrace == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all traces associated with a property, ordered by sale date (descending).
    /// </summary>
    public async Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(Guid propertyId, CancellationToken cancellationToken = default)
    {
        return await _context.PropertyTraces
            .AsNoTracking()
            .Where(pt => pt.IdProperty == propertyId)
            .OrderByDescending(pt => pt.DateSale)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a new property trace record.
    /// </summary>
    public async Task<PropertyTrace> AddAsync(PropertyTrace propertyTrace, CancellationToken cancellationToken = default)
    {
        await _context.PropertyTraces.AddAsync(propertyTrace, cancellationToken);
        return propertyTrace;
    }
}
