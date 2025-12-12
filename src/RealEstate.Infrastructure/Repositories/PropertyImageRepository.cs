using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

/// <summary>
/// Repository for managing property image entities using Entity Framework Core.
/// </summary>
public class PropertyImageRepository : IPropertyImageRepository
{
    private readonly RealEstateDbContext _context;

    /// <summary>
    /// Creates a new instance of <see cref="PropertyImageRepository"/>.
    /// </summary>
    public PropertyImageRepository(RealEstateDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a property image by its ID.
    /// </summary>
    public async Task<PropertyImage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.PropertyImages
            .AsNoTracking()
            .FirstOrDefaultAsync(pi => pi.IdPropertyImage == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves enabled images for a specific property, ordered by creation date (descending).
    /// </summary>
    public async Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(Guid propertyId, CancellationToken cancellationToken = default)
    {
        return await _context.PropertyImages
            .AsNoTracking()
            .Where(pi => pi.IdProperty == propertyId && pi.Enabled)
            .OrderByDescending(pi => pi.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a new property image.
    /// </summary>
    public async Task<PropertyImage> AddAsync(PropertyImage propertyImage, CancellationToken cancellationToken = default)
    {
        await _context.PropertyImages.AddAsync(propertyImage, cancellationToken);
        return propertyImage;
    }

    /// <summary>
    /// Updates an existing property image.
    /// </summary>
    public Task UpdateAsync(PropertyImage propertyImage, CancellationToken cancellationToken = default)
    {
        _context.PropertyImages.Update(propertyImage);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Deletes a property image by ID.
    /// </summary>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var image = await _context.PropertyImages.FindAsync(new object[] { id }, cancellationToken);
        if (image != null)
        {
            _context.PropertyImages.Remove(image);
        }
    }
}
