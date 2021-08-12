using FluentValidation;

namespace DeliveryWebApp.Application.Restaurateurs.Commands.DeleteRestaurateur
{
    public class DeleteRestaurateurCommandValidator : AbstractValidator<DeleteRestaurateurCommand>
    {
        public DeleteRestaurateurCommandValidator()
        {
            RuleFor(r => r.Id).GreaterThan(0).NotEmpty();
        }
    }
}
