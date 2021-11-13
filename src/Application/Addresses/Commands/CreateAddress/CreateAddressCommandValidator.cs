using FluentValidation;

namespace DeliveryWebApp.Application.Addresses.Commands.CreateAddress
{
    public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressCommandValidator()
        {
            RuleFor(a => a.PlaceName).NotEmpty().MaximumLength(120);

            RuleFor(a => a.Latitude).NotEmpty();

            RuleFor(a => a.Longitude).NotEmpty();
        }
    }
}
