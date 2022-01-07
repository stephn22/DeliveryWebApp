using FluentValidation;

namespace DeliveryWebApp.Application.BasketItems.Commands.DeleteBasketItem;

public class DeleteBasketItemCommandValidator : AbstractValidator<DeleteBasketItemCommand>
{
    public DeleteBasketItemCommandValidator()
    {
        RuleFor(b => b.Id).GreaterThan(0).NotEmpty();
    }
}