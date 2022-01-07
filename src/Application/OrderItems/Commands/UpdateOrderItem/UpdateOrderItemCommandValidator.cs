using FluentValidation;

namespace DeliveryWebApp.Application.OrderItems.Commands.UpdateOrderItem;

public class UpdateOrderItemCommandValidator : AbstractValidator<UpdateOrderItemCommand>
{
    public UpdateOrderItemCommandValidator()
    {
        RuleFor(o => o.Id).GreaterThan(0).NotEmpty();

        RuleFor(o => o.ProductPrice).GreaterThan(0.00M);

        RuleFor(o => o.Quantity).GreaterThan(0);

        RuleFor(o => o.Discount).GreaterThanOrEqualTo(0);
    }
}