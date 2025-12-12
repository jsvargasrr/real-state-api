namespace RealEstate.Application.DTOs;

public record ReservationDto(
    Guid IdReservation,
    Guid IdProperty,
    string GuestName,
    string GuestEmail,
    DateTime CheckIn,
    DateTime CheckOut,
    int Guests,
    decimal TotalPrice,
    string Status,
    DateTime CreatedAt
);

public record CreateReservationRequest(
    string GuestName,
    string GuestEmail,
    DateTime CheckIn,
    DateTime CheckOut,
    int Guests
);

public record ReservationDateRange(
    DateTime CheckIn,
    DateTime CheckOut
);
