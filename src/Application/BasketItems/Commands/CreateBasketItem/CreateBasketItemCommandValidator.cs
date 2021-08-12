using FluentValidation;

namespace DeliveryWebApp.Application.BasketItems.Commands.CreateBasketItem
{
    public class CreateBasketItemCommandValidator : AbstractValidator<CreateBasketItemCommand>
    {
        public CreateBasketItemCommandValidator()
        {
            RuleFor(b => b.Product).NotEmpty();

            RuleFor(b => b.Quantity).GreaterThan(0).NotEmpty();

            RuleFor(b => b.Basket).NotEmpty();
        }
    }
}