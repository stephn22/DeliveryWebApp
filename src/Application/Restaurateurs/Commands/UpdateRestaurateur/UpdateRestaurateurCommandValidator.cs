using FluentValidation;

namespace DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur
{
    public class UpdateRestaurateurCommandValidator : AbstractValidator<UpdateRestaurateurCommand>
    {
        public UpdateRestaurateurCommandValidator()
        {
            RuleFor(r => r.Id).GreaterThan(0);
        }
    }
}
