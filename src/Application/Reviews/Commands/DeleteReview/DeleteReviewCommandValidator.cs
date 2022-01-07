using FluentValidation;

namespace DeliveryWebApp.Application.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator()
    {
        RuleFor(r => r.Id).GreaterThan(0).NotEmpty();
    }
}