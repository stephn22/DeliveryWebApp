using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.BasketItems.Commands.CreateBasketItem
{
    public class CreateBasketItemCommand : IRequest<BasketItem>
    {
        public Basket Basket { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateBasketItemCommandHandler : IRequestHandler<CreateBasketItemCommand, BasketItem>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public CreateBasketItemCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<BasketItem> Handle(CreateBasketItemCommand request, CancellationToken cancellationToken)
        {
            var entity = new BasketItem
            {
                BasketId = request.Basket.Id,
                ProductId = request.Product.Id,
                Discount = request.Product.Discount,
                ProductPrice = request.Product.Price,
                Quantity = request.Quantity
            };

            var newQuantity = request.Product.Quantity - request.Quantity; // decrease product quantity

            await _mediator.Send(new UpdateProductCommand
            {
                Id = request.Product.Id,
                Quantity = newQuantity
            }, cancellationToken);

            _context.BasketItems.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
