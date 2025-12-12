using FluentValidation;
using RealEstate.Application.DTOs;

namespace RealEstate.Application.Validators;

/// <summary>
/// Validates the request to update a property's price.
/// </summary>
public class ChangePriceValidator : AbstractValidator<ChangePriceRequest>
{
    /// <summary>
    /// Defines validation rules for <see cref="ChangePriceRequest"/>.
    /// </summary>
    public ChangePriceValidator()
    {
        RuleFor(x => x.NewPrice)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
