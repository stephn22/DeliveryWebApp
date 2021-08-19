using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.OrderItems.Extensions;
using DeliveryWebApp.Application.OrderItems.Queries;
using DeliveryWebApp.Application.Products.Extensions;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Orders.Extensions
{
    public static class OrderExtensions
    {
        public static async Task<decimal> GetOrderTotalPrice(this Order order, IMediator mediator, IApplicationDbContext context)
        {
            var tot = 0.00M;

            order.OrderItems = await mediator.Send(new GetOrderItemsQuery
            {
                OrderId = order.Id
            });

            if (order.OrderItems == null)
            {
                return tot;
            }

            foreach (var item in order.OrderItems)
            {
                var product = await item.GetProduct(context);

                if (product != null)
                {
                    tot += (product.DiscountedPrice() * item.Quantity);
                }
            }

            return tot;
        }

        public static async Task<Customer> GetCustomerAsync(this Order order, IApplicationDbContext context)
        {
            var customer = await context.Customers.FindAsync(order.CustomerId);
            return customer;
        }

        public static async Task<Restaurateur> GetRestaurateurAsync(this Order order, IApplicationDbContext context)
        {
            var restaurateur = await context.Restaurateurs.FindAsync(order.RestaurateurId);
            return restaurateur;
        }

        public static async Task<Address> GetAddressAsync(this Order order, IApplicationDbContext context)
        {
            var address = await context.Addresses.FindAsync(order.AddressId);
            return address;
        }
    }
}
