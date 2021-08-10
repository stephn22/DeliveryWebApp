using FluentValidation;

namespace DeliveryWebApp.Application.Riders.Commands.DeleteRider
{
    public class DeleteRiderCommandValidator : AbstractValidator<DeleteRiderCommand>
    {
        public DeleteRiderCommandValidator()
        {
            RuleFor(r => r.Rider).NotEmpty();
        }
    }
}
