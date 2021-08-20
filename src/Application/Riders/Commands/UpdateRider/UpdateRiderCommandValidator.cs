using FluentValidation;

namespace DeliveryWebApp.Application.Riders.Commands.UpdateRider
{
    public class UpdateRiderCommandValidator : AbstractValidator<UpdateRiderCommand>
    {
        public UpdateRiderCommandValidator()
        {
            RuleFor(r => r.Id).GreaterThan(0).NotEmpty();

            RuleFor(r => r.DeliveryCredit).GreaterThan(0.00M).NotEmpty();
        }
    }
}
