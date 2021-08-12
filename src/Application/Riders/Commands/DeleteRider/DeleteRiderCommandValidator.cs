using FluentValidation;

namespace DeliveryWebApp.Application.Riders.Commands.DeleteRider
{
    public class DeleteRiderCommandValidator : AbstractValidator<DeleteRiderCommand>
    {
        public DeleteRiderCommandValidator()
        {
            RuleFor(r => r.Id).GreaterThan(0).NotEmpty();
        }
    }
}
