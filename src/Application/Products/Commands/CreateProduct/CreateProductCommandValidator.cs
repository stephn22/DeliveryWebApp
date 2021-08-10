using FluentValidation;

namespace DeliveryWebApp.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name).MaximumLength(30).NotEmpty();

            RuleFor(p => p.Category).NotEmpty();

            RuleFor(p => p.Discount).GreaterThanOrEqualTo(0).NotNull();

            RuleFor(p => p.Price).GreaterThan(0.00M).NotEmpty();

            RuleFor(p => p.Quantity).GreaterThan(0).NotEmpty();

            RuleFor(p => p.Restaurateur).NotEmpty();
        }
    }
}
