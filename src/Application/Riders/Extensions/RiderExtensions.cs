using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Riders.Extensions
{
    public static class RiderExtensions
    {
        public static async Task<Rider> GetRiderByIdAsync(this IApplicationDbContext context, int? riderId)
        {
            try
            {
                if (riderId == null)
                {
                    throw new NullReferenceException();
                }

                return await context.Riders.Where(r => r.Id == riderId).FirstAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task<Rider> GetRiderByCustomerIdAsync(this IApplicationDbContext context, int? customerId)
        {
            try
            {
                if (customerId == null)
                {
                    throw new NullReferenceException();
                }

                return await context.Riders.Where(r => r.Customer.Id == customerId).FirstAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
