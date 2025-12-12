using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RealEstateDbContext _context;
    private IPropertyRepository? _properties;
    private IOwnerRepository? _owners;
    private IPropertyImageRepository? _propertyImages;
    private IPropertyTraceRepository? _propertyTraces;
    private IReservationRepository? _reservations;

    public UnitOfWork(RealEstateDbContext context)
    {
        _context = context;
    }

    public IPropertyRepository Properties => _properties ??= new PropertyRepository(_context);
    public IOwnerRepository Owners => _owners ??= new OwnerRepository(_context);
    public IPropertyImageRepository PropertyImages => _propertyImages ??= new PropertyImageRepository(_context);
    public IPropertyTraceRepository PropertyTraces => _propertyTraces ??= new PropertyTraceRepository(_context);
    public IReservationRepository Reservations => _reservations ??= new ReservationRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
