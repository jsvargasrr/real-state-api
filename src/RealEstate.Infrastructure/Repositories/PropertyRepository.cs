using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

/// <summary>
/// Repository for managing property entities, including retrieval, filtering, and persistence.
/// </summary>
public class PropertyRepository : IPropertyRepository
{
    private readonly RealEstateDbContext _context;

    /// <summary>
    /// Creates a new instance of <see cref="PropertyRepository"/>.
    /// </summary>
    public PropertyRepository(RealEstateDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a property by ID, including owner and traces.
    /// </summary>
    public async Task<Property?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Properties
            .Include(p => p.Owner)
            .Include(p => p.PropertyTraces)
            .FirstOrDefaultAsync(p => p.IdProperty == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a property by ID including enabled images and related data.
    /// </summary>
    public async Task<Property?> GetByIdWithImagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Properties
            .Include(p => p.Owner)
            .Include(p => p.PropertyImages.Where(pi => pi.Enabled))
            .Include(p => p.PropertyTraces)
            .FirstOrDefaultAsync(p => p.IdProperty == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a paginated and filtered list of properties.
    /// </summary>
    public async Task<(IEnumerable<Property> Properties, int TotalCount)> GetAllWithFiltersAsync(
        string? name = null,
        string? address = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int? year = null,
        Guid? ownerId = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Properties
            .Include(p => p.Owner)
            .Include(p => p.PropertyImages.Where(pi => pi.Enabled))
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));

        if (!string.IsNullOrWhiteSpace(address))
            query = query.Where(p => p.Address.ToLower().Contains(address.ToLower()));

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        if (year.HasValue)
            query = query.Where(p => p.Year == year.Value);

        if (ownerId.HasValue)
            query = query.Where(p => p.IdOwner == ownerId.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var properties = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (properties, totalCount);
    }

    /// <summary>
    /// Adds a new property.
    /// </summary>
    public async Task<Property> AddAsync(Property property, CancellationToken cancellationToken = default)
    {
        await _context.Properties.AddAsync(property, cancellationToken);
        return property;
    }

    /// <summary>
    /// Updates an existing property.
    /// </summary>
    public Task UpdateAsync(Property property, CancellationToken cancellationToken = default)
    {
        _context.Properties.Update(property);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Checks if a property exists by ID.
    /// </summary>
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Properties.AnyAsync(p => p.IdProperty == id, cancellationToken);
    }

    /// <summary>
    /// Checks if an internal code already exists, optionally excluding a specific property.
    /// </summary>
    public async Task<bool> CodeInternalExistsAsync(string codeInternal, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Properties.Where(p => p.CodeInternal == codeInternal);

        if (excludeId.HasValue)
            query = query.Where(p => p.IdProperty != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }
}
