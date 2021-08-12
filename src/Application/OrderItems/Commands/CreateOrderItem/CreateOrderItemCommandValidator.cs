using FluentValidation;

namespace DeliveryWebApp.Application.OrderItems.Commands.CreateOrderItem
{
    public class CreateOrderItemCommandValidator : AbstractValidator<CreateOrderItemCommand>
    {
        public CreateOrderItemCommandValidator()
        {
            RuleFor(o => o.Order).NotEmpty();

            RuleFor(o => o.Order.Id).GreaterThan(0).NotNull();

            RuleFor(o => o.BasketItem).NotEmpty();

            RuleFor(o => o.BasketItem.Id).GreaterThan(0).NotEmpty();
        }
    }
}
