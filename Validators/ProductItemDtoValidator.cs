using FluentValidation;
using RestGateway.Models.DTOs.Requests;

namespace RestGateway.Validators;

/// <summary>
/// Validator for ProductItemDto
/// </summary>
public class ProductItemDtoValidator : AbstractValidator<ProductItemDto>
{
    public ProductItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than 0");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(10000)
            .WithMessage("Quantity cannot exceed 10,000 units");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("UnitPrice must be greater than 0");
    }
}
