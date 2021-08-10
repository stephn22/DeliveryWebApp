using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using FluentValidation;
using System.Text.RegularExpressions;

namespace DeliveryWebApp.Application.Addresses.Commands.CreateAddress
{
    public class CreateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
    {
        public CreateAddressCommandValidator()
        {
            RuleFor(a => a.AddressLine1)
                .MaximumLength(80)
                .NotEmpty();

            RuleFor(a => a.AddressLine2)
                .MaximumLength(60);

            RuleFor(a => a.Number)
                .MaximumLength(10)
                .NotEmpty();

            RuleFor(a => a.City)
                .MaximumLength(15)
                .NotEmpty();

            RuleFor(a => a.StateProvince)
                .MaximumLength(15)
                .NotEmpty();

            RuleFor(a => a.PostalCode)
                .MaximumLength(10)
                .NotEmpty();

            RuleFor(a => a.Country)
                .Matches(new Regex(@"/[a-zA-Z]{2,}/gm"))
                .NotEmpty();

            RuleFor(a => a.Latitude)
                .NotEmpty()
                .NotNull();

            RuleFor(a => a.Longitude)
                .NotEmpty()
                .NotNull();
        }
    }
}
