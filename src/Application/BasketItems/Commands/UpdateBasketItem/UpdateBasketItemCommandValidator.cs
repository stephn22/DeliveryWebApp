using FluentValidation;

namespace DeliveryWebApp.Application.BasketItems.Commands.UpdateBasketItem;

public class UpdateBasketItemCommandValidator : AbstractValidator<UpdateBasketItemCommand>
{
    public UpdateBasketItemCommandValidator()
    {
        RuleFor(b => b.Id).GreaterThan(0).NotEmpty();

        RuleFor(b => b.Quantity).GreaterThan(0).NotEmpty();
    }
}