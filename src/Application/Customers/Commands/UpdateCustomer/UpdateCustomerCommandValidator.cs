using FluentValidation;

namespace DeliveryWebApp.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0).NotEmpty();

            RuleFor(c => c.Email).EmailAddress();

            RuleFor(c => c.FName).MaximumLength(40);

            RuleFor(c => c.LName).MaximumLength(40);
        }
    }
}
