using AutoMapper;
using RealEstate.Application.Common;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.UseCases.Reservations;

/// <summary>
/// Retrieves all reservations associated with a specific property.
/// </summary>
public class GetPropertyReservationsHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates a new instance of <see cref="GetPropertyReservationsHandler"/>.
    /// </summary>
    public GetPropertyReservationsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a property's reservations after verifying that the property exists.
    /// </summary>
    public async Task<Result<IEnumerable<ReservationDto>>> HandleAsync(Guid propertyId, CancellationToken cancellationToken = default)
    {
        var property = await _unitOfWork.Properties.GetByIdAsync(propertyId, cancellationToken);
        if (property == null)
            return Result<IEnumerable<ReservationDto>>.Failure("Property not found", "NOT_FOUND");

        var reservations = await _unitOfWork.Reservations.GetByPropertyIdAsync(propertyId, cancellationToken);
        var dtos = _mapper.Map<IEnumerable<ReservationDto>>(reservations);

        return Result<IEnumerable<ReservationDto>>.Success(dtos);
    }
}
