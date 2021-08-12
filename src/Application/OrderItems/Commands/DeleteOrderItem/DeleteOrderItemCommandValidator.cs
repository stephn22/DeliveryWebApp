using FluentValidation;

namespace DeliveryWebApp.Application.OrderItems.Commands.DeleteOrderItem
{
    public class DeleteOrderItemCommandValidator : AbstractValidator<DeleteOrderItemCommand>
    {
        public DeleteOrderItemCommandValidator()
        {
            RuleFor(o => o.Id).GreaterThan(0).NotEmpty();
        }
    }
}
