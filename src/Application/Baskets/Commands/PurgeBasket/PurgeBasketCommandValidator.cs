﻿using FluentValidation;

namespace DeliveryWebApp.Application.Baskets.Commands.PurgeBasket
{
    public class PurgeBasketCommandValidator : AbstractValidator<PurgeBasketCommand>
    {
        public PurgeBasketCommandValidator()
        {
            RuleFor(b => b.Basket).NotEmpty();
        }
    }
}
