using System;
using DeliveryWebApp.Domain.Constants;
using FluentValidation;

namespace DeliveryWebApp.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(o => o.Id).GreaterThan(0).NotEmpty();

            RuleFor(o => o.DeliveryDate).GreaterThanOrEqualTo(DateTime.UtcNow);

            RuleFor(o => o.OrderStatus).NotEmpty();
        }
    }
}
