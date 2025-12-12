using RealEstate.Application.Common;
using RealEstate.Application.DTOs;

namespace RealEstate.Application.Interfaces;

public interface IOwnerService
{
    Task<Result<OwnerDto>> CreateOwnerAsync(CreateOwnerRequest request, CancellationToken cancellationToken = default);
    Task<Result<OwnerDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<OwnerDto>>> GetAllAsync(CancellationToken cancellationToken = default);
}
