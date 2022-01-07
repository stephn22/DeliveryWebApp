using FluentValidation;

namespace DeliveryWebApp.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(c => c.ApplicationUserFk).Length(36).NotEmpty();

        RuleFor(c => c.Email).EmailAddress().NotEmpty();

        RuleFor(c => c.FirstName).MaximumLength(40).NotEmpty();

        RuleFor(c => c.LastName).MaximumLength(40).NotEmpty();
    }
}