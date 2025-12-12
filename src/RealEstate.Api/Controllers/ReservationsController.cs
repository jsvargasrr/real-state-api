using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.UseCases.Reservations;

namespace RealEstate.Api.Controllers;

[ApiController]
[Route("api/properties/{propertyId:guid}/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly CreateReservationHandler _createHandler;
    private readonly GetPropertyReservationsHandler _getHandler;
    private readonly CancelReservationHandler _cancelHandler;

    /// <summary>
    /// Controller for managing property reservations.
    /// </summary>
    public ReservationsController(
        CreateReservationHandler createHandler,
        GetPropertyReservationsHandler getHandler,
        CancelReservationHandler cancelHandler)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _cancelHandler = cancelHandler;
    }

    /// <summary>
    /// Retrieves reservations for a property.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReservations(Guid propertyId, CancellationToken cancellationToken)
    {
        var result = await _getHandler.HandleAsync(propertyId, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "NOT_FOUND" => NotFound(new { error = result.Error }),
                _ => BadRequest(new { error = result.Error })
            };
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a reservation ensuring availability and date validity.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateReservation(Guid propertyId, [FromBody] CreateReservationRequest request, CancellationToken cancellationToken)
    {
        var result = await _createHandler.HandleAsync(propertyId, request, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "NOT_FOUND" => NotFound(new { error = result.Error }),
                "DATE_CONFLICT" => Conflict(new { error = result.Error }),
                _ => BadRequest(new { error = result.Error })
            };
        }

        return CreatedAtAction(nameof(GetReservations), new { propertyId }, result.Data);
    }

    /// <summary>
    /// Cancels a reservation.
    /// </summary>
    [HttpDelete("{reservationId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelReservation(Guid propertyId, Guid reservationId, CancellationToken cancellationToken)
    {
        var result = await _cancelHandler.HandleAsync(reservationId, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "NOT_FOUND" => NotFound(new { error = result.Error }),
                _ => BadRequest(new { error = result.Error })
            };
        }

        return NoContent();
    }
}
