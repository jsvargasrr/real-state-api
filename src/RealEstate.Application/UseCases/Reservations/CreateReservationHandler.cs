using AutoMapper;
using RealEstate.Application.Common;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.UseCases.Reservations;

/// <summary>
/// Handles the creation of a reservation, including validation, availability checks, and price calculation.
/// </summary>
public class CreateReservationHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates a new instance of <see cref="CreateReservationHandler"/>.
    /// </summary>
    public CreateReservationHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a reservation after validating input, ensuring property availability, and calculating total price.
    /// </summary>
    public async Task<Result<ReservationDto>> HandleAsync(Guid propertyId, CreateReservationRequest request, CancellationToken cancellationToken = default)
    {
        // Basic validations
        if (string.IsNullOrWhiteSpace(request.GuestName))
            return Result<ReservationDto>.Failure("Guest name is required", "VALIDATION_ERROR");

        if (string.IsNullOrWhiteSpace(request.GuestEmail))
            return Result<ReservationDto>.Failure("Guest email is required", "VALIDATION_ERROR");

        if (request.CheckIn >= request.CheckOut)
            return Result<ReservationDto>.Failure("Check-out must be after check-in", "VALIDATION_ERROR");

        if (request.CheckIn < DateTime.Today)
            return Result<ReservationDto>.Failure("Check-in cannot be in the past", "VALIDATION_ERROR");

        if (request.Guests < 1)
            return Result<ReservationDto>.Failure("At least 1 guest is required", "VALIDATION_ERROR");

        // Property existence check
        var property = await _unitOfWork.Properties.GetByIdAsync(propertyId, cancellationToken);
        if (property == null)
            return Result<ReservationDto>.Failure("Property not found", "NOT_FOUND");

        // Availability check
        var hasConflict = await _unitOfWork.Reservations.HasConflictAsync(propertyId, request.CheckIn, request.CheckOut, null, cancellationToken);
        if (hasConflict)
            return Result<ReservationDto>.Failure("Property is not available for selected dates", "DATE_CONFLICT");

        // Basic price calculation (prorated monthly rate)
        var nights = (request.CheckOut - request.CheckIn).Days;
        var totalPrice = property.Price * nights / 30;

        // Reservation creation
        var reservation = new Reservation
        {
            IdReservation = Guid.NewGuid(),
            IdProperty = propertyId,
            GuestName = request.GuestName,
            GuestEmail = request.GuestEmail,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            Guests = request.Guests,
            TotalPrice = totalPrice,
            Status = "confirmed",
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Reservations.AddAsync(reservation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<ReservationDto>(reservation);
        return Result<ReservationDto>.Success(dto);
    }
}
