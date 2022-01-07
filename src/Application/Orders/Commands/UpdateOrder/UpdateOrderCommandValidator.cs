using FluentValidation;
using System;

namespace DeliveryWebApp.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(o => o.Id).GreaterThan(0).NotEmpty();

        RuleFor(o => o.DeliveryDate).GreaterThanOrEqualTo(DateTime.Today);

        RuleFor(o => o.OrderStatus).NotEmpty();
    }
}