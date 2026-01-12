using FluentValidation;
using RestGateway.Models.DTOs.Requests;

namespace RestGateway.Validators;

/// <summary>
/// Validator for UpdateOrderStatusRequestDto
/// </summary>
public class UpdateOrderStatusRequestDtoValidator : AbstractValidator<UpdateOrderStatusRequestDto>
{
    private static readonly string[] ValidStatuses = { "CREATED", "PAID", "SHIPPED", "DELIVERED" };

    public UpdateOrderStatusRequestDtoValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0)
            .WithMessage("OrderId must be greater than 0");

        RuleFor(x => x.NewStatus)
            .NotEmpty()
            .Must(status => ValidStatuses.Contains(status.ToUpper()))
            .WithMessage($"NewStatus must be one of: {string.Join(", ", ValidStatuses)}");
    }
}
