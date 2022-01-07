using FluentValidation;

namespace DeliveryWebApp.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(o => o.Id).GreaterThan(0).NotEmpty();
    }
}