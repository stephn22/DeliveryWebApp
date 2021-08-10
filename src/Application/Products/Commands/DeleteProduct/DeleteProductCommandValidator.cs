using FluentValidation;

namespace DeliveryWebApp.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(p => p.Product).NotEmpty();
        }
    }
}
