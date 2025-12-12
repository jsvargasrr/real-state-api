using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Reservation>> GetByPropertyIdAsync(Guid propertyId, CancellationToken cancellationToken = default);
    Task<bool> HasConflictAsync(Guid propertyId, DateTime checkIn, DateTime checkOut, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<Reservation> AddAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task DeleteAsync(Reservation reservation, CancellationToken cancellationToken = default);
}
