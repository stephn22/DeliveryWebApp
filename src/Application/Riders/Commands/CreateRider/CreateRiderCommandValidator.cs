using FluentValidation;

namespace DeliveryWebApp.Application.Riders.Commands.CreateRider;

public class CreateRiderCommandValidator : AbstractValidator<CreateRiderCommand>
{
    public CreateRiderCommandValidator()
    {
        RuleFor(r => r.Customer).NotEmpty();

        RuleFor(r => r.DeliveryCredit).GreaterThan(0.00M).NotEmpty();
    }
}