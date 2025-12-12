namespace RealEstate.Application.DTOs;

public record PropertyTraceDto(
    Guid IdPropertyTrace,
    Guid IdProperty,
    DateTime DateSale,
    string Name,
    decimal Value,
    decimal Tax
);
