using FluentValidation;
using RealEstate.Application.DTOs;

namespace RealEstate.Application.Validators;

/// <summary>
/// Validates data for adding an image to a property.
/// </summary>
public class AddPropertyImageValidator : AbstractValidator<AddPropertyImageRequest>
{
    /// <summary>
    /// Defines validation rules for <see cref="AddPropertyImageRequest"/>.
    /// </summary>
    public AddPropertyImageValidator()
    {
        RuleFor(x => x.File)
            .NotEmpty().WithMessage("File path is required")
            .MaximumLength(1000).WithMessage("File path cannot exceed 1000 characters");
    }
}
