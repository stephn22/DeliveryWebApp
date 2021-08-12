using FluentValidation;

namespace DeliveryWebApp.Application.Products.Commands.UpdateProducts
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).NotEmpty();

            RuleFor(p => p.Name).MaximumLength(30);

            RuleFor(p => p.Category).MaximumLength(20);

            RuleFor(p => p.Discount).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);

            RuleFor(p => p.Price).GreaterThan(0.00M);

            RuleFor(p => p.Quantity).GreaterThan(0);
        }
    }
}
