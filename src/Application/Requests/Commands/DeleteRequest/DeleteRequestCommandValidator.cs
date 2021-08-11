using FluentValidation;

namespace DeliveryWebApp.Application.Requests.Commands.DeleteRequest
{
    public class DeleteRequestCommandValidator : AbstractValidator<DeleteRequestCommand>
    {
        public DeleteRequestCommandValidator()
        {
            RuleFor(r => r.Request).NotEmpty();
        }
    }
}
