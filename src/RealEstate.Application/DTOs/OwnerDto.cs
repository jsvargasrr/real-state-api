namespace RealEstate.Application.DTOs;

public record OwnerDto(
    Guid IdOwner,
    string Name,
    string Address,
    string? Photo,
    DateTime Birthday
);

public record CreateOwnerRequest(
    string Name,
    string Address,
    string? Photo,
    DateTime Birthday
);
