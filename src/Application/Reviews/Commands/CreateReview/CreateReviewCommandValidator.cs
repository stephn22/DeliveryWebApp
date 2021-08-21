using FluentValidation;

namespace DeliveryWebApp.Application.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(r => r.Customer).NotEmpty();

            RuleFor(r => r.Restaurateur).NotEmpty();

            RuleFor(r => r.Rating).GreaterThanOrEqualTo(1).LessThanOrEqualTo(5).NotEmpty();

            RuleFor(r => r.Title).MaximumLength(30).NotEmpty();

            RuleFor(r => r.Text).MaximumLength(250);
        }
    }
}
