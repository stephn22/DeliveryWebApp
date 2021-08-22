using FluentValidation;

namespace DeliveryWebApp.Application.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
    {
        public UpdateAddressCommandValidator()
        {
            RuleFor(a => a.Id).GreaterThan(0).NotEmpty();

            RuleFor(a => a.PlaceName).NotEmpty().MaximumLength(120);

            RuleFor(a => a.Latitude).NotEmpty();

            RuleFor(a => a.Longitude).NotEmpty();
        }
    }
}