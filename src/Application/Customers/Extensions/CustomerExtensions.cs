using System;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Application.Customers.Extensions
{
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
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static async Task<Customer> GetCustomerByIdAsync(this IApplicationDbContext context, int? customerId)
        {
            try
            {
                if (customerId == null)
                {
                    throw new NullReferenceException();
                }

                return await context.Customers.Where(c => c.Id == customerId).FirstAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

}
