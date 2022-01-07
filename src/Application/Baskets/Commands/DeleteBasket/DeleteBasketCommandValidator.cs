using FluentValidation;

namespace DeliveryWebApp.Application.Baskets.Commands.DeleteBasket;

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(b => b.Id).GreaterThan(0).NotEmpty();
    }
}