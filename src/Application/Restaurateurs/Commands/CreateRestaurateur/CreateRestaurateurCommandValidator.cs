using FluentValidation;

namespace DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur
{
    public class CreateRestaurateurCommandValidator : AbstractValidator<CreateRestaurateurCommand>
    {
        public CreateRestaurateurCommandValidator()
        {
            RuleFor(r => r.Customer).NotEmpty();
        }
    }
}
