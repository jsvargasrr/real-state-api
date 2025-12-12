using FluentValidation;
using RealEstate.Application.DTOs;

namespace RealEstate.Application.Validators;

/// <summary>
/// Validates data required to create a new property.
/// </summary>
public class CreatePropertyValidator : AbstractValidator<CreatePropertyRequest>
{
    /// <summary>
    /// Defines validation rules for <see cref="CreatePropertyRequest"/>.
    /// </summary>
    public CreatePropertyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.CodeInternal)
            .NotEmpty().WithMessage("CodeInternal is required")
            .MaximumLength(50).WithMessage("CodeInternal cannot exceed 50 characters");

        RuleFor(x => x.Year)
            .InclusiveBetween(1800, DateTime.Now.Year + 10)
            .WithMessage($"Year must be between 1800 and {DateTime.Now.Year + 10}");

        RuleFor(x => x.IdOwner)
            .NotEmpty().WithMessage("Owner is required");
    }
}
