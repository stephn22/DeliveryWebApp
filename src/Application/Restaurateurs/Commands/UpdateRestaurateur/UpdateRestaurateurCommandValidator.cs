using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur
{
    public class UpdateRestaurateurCommandValidator : AbstractValidator<UpdateRestaurateurCommand>
    {
        public UpdateRestaurateurCommandValidator()
        {
            RuleFor(r => r.Restaurant)
                .NotNull();
        }
    }
}
