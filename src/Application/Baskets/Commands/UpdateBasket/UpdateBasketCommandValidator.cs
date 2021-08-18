using FluentValidation;

namespace DeliveryWebApp.Application.Baskets.Commands.UpdateBasket
{
    public class UpdateBasketCommandValidator : AbstractValidator<UpdateBasketCommand>
    {
        public UpdateBasketCommandValidator()
        {
            RuleFor(b => b.Quantity).GreaterThan(0).NotEmpty();

            RuleFor(b => b.Product).NotEmpty();

            RuleFor(b => b.Basket).NotEmpty();

            RuleFor(b => b.RestaurateurId).GreaterThan(0).NotEmpty();
        }
    }
}
