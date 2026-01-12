using FluentValidation;
using RestGateway.Models.DTOs.Requests;

namespace RestGateway.Validators;

/// <summary>
/// Validator for AddressDto
/// </summary>
public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Street is required and cannot exceed 200 characters");

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("City is required and cannot exceed 100 characters");

        RuleFor(x => x.State)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("State is required and cannot exceed 100 characters");

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .MaximumLength(20)
            .WithMessage("ZipCode is required and cannot exceed 20 characters");

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Country is required and cannot exceed 100 characters");
    }
}
