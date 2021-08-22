using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Reviews.Extensions
{
    public static class ReviewExtensions
    {
        public static async Task<Customer> GetCustomerAsync(this Review review, IApplicationDbContext context)
        {
            try
            {
                var customer = await context.Customers.FirstAsync(c => c.Id == review.CustomerId);
                return customer;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
