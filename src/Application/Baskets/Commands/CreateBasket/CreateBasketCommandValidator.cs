using FluentValidation;

namespace DeliveryWebApp.Application.Baskets.Commands.CreateBasket;

public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(b => b.Customer).NotEmpty();
    }
}