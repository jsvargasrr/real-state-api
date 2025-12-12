namespace RealEstate.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPropertyRepository Properties { get; }
    IOwnerRepository Owners { get; }
    IPropertyImageRepository PropertyImages { get; }
    IPropertyTraceRepository PropertyTraces { get; }
    IReservationRepository Reservations { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
