using FluentValidation;

namespace DeliveryWebApp.Application.OrderItems.Commands.CreateOrderItem
{
    public class CreateOrderItemCommandValidator : AbstractValidator<CreateOrderItemCommand>
    {
        public CreateOrderItemCommandValidator()
        {
            RuleFor(o => o.OrderId).GreaterThan(0).NotNull();

            RuleFor(o => o.BasketItemId).GreaterThan(0).NotEmpty();
        }
    }
}
