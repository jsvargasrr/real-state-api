namespace RealEstate.Application.DTOs;

public record PropertyImageDto(
    Guid IdPropertyImage,
    Guid IdProperty,
    string File,
    bool Enabled
);

public record AddPropertyImageRequest(
    string File,
    bool Enabled = true
);
