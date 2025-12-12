using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

/// <summary>
/// Repository for managing reservation entities, including retrieval, conflict checks, and persistence.
/// </summary>
public class ReservationRepository : IReservationRepository
{
    private readonly RealEstateDbContext _context;

    /// <summary>
    /// Creates a new instance of <see cref="ReservationRepository"/>.
    /// </summary>
    public ReservationRepository(RealEstateDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets a reservation by ID, including its associated property.
    /// </summary>
    public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .Include(r => r.Property)
            .FirstOrDefaultAsync(r => r.IdReservation == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves active (non-cancelled) reservations for a specific property.
    /// </summary>
    public async Task<IEnumerable<Reservation>> GetByPropertyIdAsync(Guid propertyId, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .Where(r => r.IdProperty == propertyId && r.Status != "cancelled")
            .OrderBy(r => r.CheckIn)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Checks whether the specified date range conflicts with an existing reservation.
    /// </summary>
    public async Task<bool> HasConflictAsync(Guid propertyId, DateTime checkIn, DateTime checkOut, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Reservations
            .Where(r => r.IdProperty == propertyId && r.Status != "cancelled");

        if (excludeId.HasValue)
        {
            query = query.Where(r => r.IdReservation != excludeId.Value);
        }

        return await query.AnyAsync(
            r => checkIn < r.CheckOut && checkOut > r.CheckIn,
            cancellationToken
        );
    }

    /// <summary>
    /// Adds a new reservation.
    /// </summary>
    public async Task<Reservation> AddAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        await _context.Reservations.AddAsync(reservation, cancellationToken);
        return reservation;
    }

    /// <summary>
    /// Removes a reservation from the database.
    /// </summary>
    public Task DeleteAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        _context.Reservations.Remove(reservation);
        return Task.CompletedTask;
    }
}
