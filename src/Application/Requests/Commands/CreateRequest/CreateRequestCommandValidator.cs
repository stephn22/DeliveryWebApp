using DeliveryWebApp.Domain.Constants;
using FluentValidation;

namespace DeliveryWebApp.Application.Requests.Commands.CreateRequest
{
    public class CreateRequestCommandValidator : AbstractValidator<CreateRequestCommand>
    {
        public CreateRequestCommandValidator()
        {
            RuleFor(r => r.CustomerId).NotEmpty();

            RuleFor(r => r.Role).NotEmpty();

            RuleFor(r => r.Status).Equal(RequestStatus.Idle);
        }
    }
}
