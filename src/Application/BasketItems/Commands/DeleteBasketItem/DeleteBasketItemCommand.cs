using AutoMapper;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;

namespace DeliveryWebApp.Application.BasketItems.Commands.DeleteBasketItem
{
    public class DeleteBasketItemCommand : IRequest<BasketItem>
    {
        public BasketItem BasketItem { get; set; }
    }

    public class DeleteBasketItemCommandHandler : IRequestHandler<DeleteBasketItemCommand, BasketItem>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteBasketItemCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BasketItem> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<BasketItem>(request.BasketItem);

            if (entity == null)
            {
                throw new NotFoundException(nameof(BasketItem), request.BasketItem.Id);
            }

            _context.BasketItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }

}
