using RealEstate.Application.Common;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.UseCases.Reservations;

/// <summary>
/// Handles reservation cancellation by validating existence and updating status.
/// </summary>
public class CancelReservationHandler
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Creates a new instance of <see cref="CancelReservationHandler"/>.
    /// </summary>
    public CancelReservationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Cancels a reservation if it exists.
    /// </summary>
    public async Task<Result<bool>> HandleAsync(Guid reservationId, CancellationToken cancellationToken = default)
    {
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId, cancellationToken);
        if (reservation == null)
            return Result<bool>.Failure("Reservation not found", "NOT_FOUND");

        reservation.Status = "cancelled";
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
