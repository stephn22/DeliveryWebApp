using FluentValidation;

namespace DeliveryWebApp.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(o => o.Customer).NotEmpty();

            RuleFor(o => o.RestaurateurId).GreaterThan(0).NotEmpty();

            RuleFor(o => o.BasketItems).NotEmpty();
        }
    }
}
