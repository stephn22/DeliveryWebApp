using FluentValidation;

namespace DeliveryWebApp.Application.Addresses.Commands.DeleteAddress
{
    public class DeleteAddressValidator : AbstractValidator<DeleteAddressCommand>
    {
        public DeleteAddressValidator()
        {
            RuleFor(a => a.Id)
                .GreaterThan(0)
                .NotEmpty();
        }
    }
}
