namespace RealEstate.Application.DTOs;

public record PropertyDto(
    Guid IdProperty,
    string Name,
    string Address,
    decimal Price,
    string CodeInternal,
    int Year,
    Guid IdOwner,
    string? OwnerName,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IEnumerable<PropertyImageDto>? Images = null
);

public record CreatePropertyRequest(
    string Name,
    string Address,
    decimal Price,
    string CodeInternal,
    int Year,
    Guid IdOwner
);

public record UpdatePropertyRequest(
    string Name,
    string Address,
    string CodeInternal,
    int Year,
    Guid IdOwner
);

public record ChangePriceRequest(
    decimal NewPrice
);

public record PropertyFilterRequest(
    string? Name = null,
    string? Address = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    int? Year = null,
    Guid? OwnerId = null,
    int Page = 1,
    int PageSize = 10
);
