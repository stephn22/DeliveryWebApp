using FluentValidation;

namespace DeliveryWebApp.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(o => o.Customer).NotEmpty();

            RuleFor(c => c.Customer.Id).NotEqual(c => c.Restaurateur.CustomerId).NotEmpty();

            RuleFor(c => c.Customer.Id).GreaterThan(0).NotEmpty();

            RuleFor(o => o.Restaurateur).NotEmpty();

            RuleFor(o => o.Restaurateur.Id).GreaterThan(0).NotEmpty();
        }
    }
}
