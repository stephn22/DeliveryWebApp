using AutoMapper;
using DeliveryWebApp.Application.Addresses.Queries.GetAddresses;
using DeliveryWebApp.Application.Baskets.Commands.DeleteBasket;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Restaurateurs.Commands.DeleteRestaurateur;
using DeliveryWebApp.Application.Reviews.Commands.DeleteReview;
using DeliveryWebApp.Application.Reviews.Queries.GetReviews;
using DeliveryWebApp.Application.Riders.Commands.DeleteRider;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommand : IRequest<Customer>
{
    public Customer Customer { get; set; }
}

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Customer>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public DeleteCustomerCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;

        _mediator = mediator;
    }

    public async Task<Customer> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Customer>(request.Customer);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Customer), request.Customer.Id);
        }

        // search if customer is also a restaurateur or a rider and delete it
        try
        {
            var restaurateur =
                await _context.Restaurateurs.FirstAsync(r => r.CustomerId == entity.Id, cancellationToken);

            await _mediator.Send(new DeleteRestaurateurCommand
            {
                Id = restaurateur.Id
            }, cancellationToken);
        }
        catch (InvalidOperationException)
        {
        }
        try
        {
            var rider =
                await _context.Riders.FirstAsync(r => r.CustomerId == entity.Id, cancellationToken);

            await _mediator.Send(new DeleteRiderCommand
            {
                Id = rider.Id
            }, cancellationToken);
        }
        catch (InvalidOperationException)
        {
        }

        // search if customer has basket and delete it
        try
        {
            var basket = await _context.Baskets.FirstAsync(b => b.CustomerId == entity.Id, cancellationToken);

            await _mediator.Send(new DeleteBasketCommand
            {
                Id = basket.Id
            }, cancellationToken);
        }
        catch (InvalidOperationException)
        {
        }

        // search if customer has addresses and delete it
        var addresses = await _mediator.Send(new GetAddressesQuery
        {
            CustomerId = entity.Id
        }, cancellationToken);

        if (addresses is { Count: > 0 })
        {
            _context.Addresses.RemoveRange(addresses);
        }

        // search if customer has reviews and delete them
        var reviews = await _mediator.Send(new GetReviewsQuery
        {
            CustomerId = entity.Id
        }, cancellationToken);

        if (reviews is { Count: > 0 })
        {
            foreach (var review in reviews)
            {
                try
                {
                    await _mediator.Send(new DeleteReviewCommand
                    {
                        Id = review.Id
                    }, cancellationToken);
                }
                catch (NotFoundException)
                {
                }
            }
        }

        _context.Customers.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}