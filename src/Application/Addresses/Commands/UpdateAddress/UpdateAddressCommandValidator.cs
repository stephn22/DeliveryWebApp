using System.Text.RegularExpressions;
using FluentValidation;

namespace DeliveryWebApp.Application.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
    {
        public UpdateAddressCommandValidator()
        {
            RuleFor(a => a.Id).GreaterThan(0).NotEmpty();

            RuleFor(a => a.AddressLine1).MaximumLength(80).NotEmpty();

            RuleFor(a => a.AddressLine2).MaximumLength(60).NotEmpty();

            RuleFor(a => a.Number).MaximumLength(10).NotEmpty();

            RuleFor(a => a.City).MaximumLength(15).NotEmpty();

            RuleFor(a => a.StateProvince).MaximumLength(15).NotEmpty();

            RuleFor(a => a.PostalCode).MaximumLength(10).NotEmpty();

            RuleFor(a => a.Country).Matches(new Regex(@"/[a-zA-Z]{2,}/gm")).NotEmpty();

            RuleFor(a => a.Latitude).NotEmpty();

            RuleFor(a => a.Longitude).NotEmpty();
        }
    }
}