using FluentValidation;
using RestGateway.Models.DTOs.Requests;

namespace RestGateway.Validators;

/// <summary>
/// Validator for CreateOrderRequestDto
/// </summary>
public class CreateOrderRequestDtoValidator : AbstractValidator<CreateOrderRequestDto>
{
    public CreateOrderRequestDtoValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage("ClientId must be greater than 0");

        RuleFor(x => x.Products)
            .NotEmpty()
            .WithMessage("At least one product is required");

        RuleForEach(x => x.Products)
            .SetValidator(new ProductItemDtoValidator());

        RuleFor(x => x.Address)
            .NotNull()
            .SetValidator(new AddressDtoValidator());
    }
}
