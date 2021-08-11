using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.OrderItems.Queries;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Orders.Extensions
{
    public static class OrderExtensions
    {
        public static async Task<decimal> GetOrderTotalPrice(this Order order, IMediator mediator)
        {
            const decimal tot = 0.00M;

            order.OrderItems = await mediator.Send(new GetOrderItemsQuery
            {
                OrderId = order.Id
            });

            return order.OrderItems?.Sum(item => (item.ProductPrice.DiscountedPrice(item.Discount) * item.Quantity)) ?? tot;
        }

        private static decimal DiscountedPrice(this decimal price, int discount)
        {
            if (discount != 0)
            {
                return price * ((100.000M - discount) / 100.00M);
            }

            return price;
        }
    }
}
