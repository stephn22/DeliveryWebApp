using DeliveryWebApp.Domain.Constants;
using FluentValidation;

namespace DeliveryWebApp.Application.Requests.Commands.UpdateRequest
{
    public class UpdateRequestCommandValidator : AbstractValidator<UpdateRequestCommand>
    {
        public UpdateRequestCommandValidator()
        {
            RuleFor(r => r.Id).GreaterThan(0).NotEmpty();

            RuleFor(r => r.Status).NotEmpty();
        }
    }
}
