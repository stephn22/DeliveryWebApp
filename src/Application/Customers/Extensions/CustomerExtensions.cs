using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Orders.Queries.GetOrders;
using DeliveryWebApp.Application.Reviews.Queries.GetReviews;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Customers.Extensions;

public static class CustomerExtensions
{
    /// <summary>
    /// Get the Customer instance given the identifier (Customer.Id)
    /// </summary>
    /// <param name="context">database context</param>
    /// <param name="requestId">id or user request</param>
    /// <returns>Customer instance</returns>
    public static async Task<Customer> GetCustomerByRequestIdAsync(this IApplicationDbContext context, int? requestId)
    {
        try
        {
            if (requestId == null)
            {
                throw new NullReferenceException();
            }

            return await (from request in context.Requests
                where request.Id == requestId
                select request.Customer).FirstAsync();
        }
        catch (NullReferenceException)
        {
            return null;
        }
    }

    /// <summary>
    /// Returns how many left reviews customer can post on restaurateur.
    /// Customer can do as many reviews as the difference between orders with status delivered
    /// (eg: 1 delivered 0 reviews => 1 review left)
    /// Number of delivered should always be > number of reviews
    /// </summary>
    /// <param name="customer">Customer that can review</param>
    /// <param name="mediator">IMediator</param>
    /// <returns>Number of reviews customer has left</returns>
    public static async Task<int> GetAvailableReviews(this Customer customer, IMediator mediator)
    {
        var availableReviews = 0;

        var orders = await mediator.Send(new GetOrdersQuery
        {
            CustomerId = customer.Id
        });

        var reviews = await mediator.Send(new GetReviewsQuery
        {
            CustomerId = customer.Id
        });

        if (orders is { Count: > 0 })
        {
            var deliveredList = orders.FindAll(o => o.Status == OrderStatus.Delivered);

            if (reviews is { Count: > 0 })
            {
                return Math.Abs(deliveredList.Count - reviews.Count);
            }

            availableReviews = deliveredList.Count;
        }

        return availableReviews;
    }
}