using System;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

                return await context.Riders.Where(c => c.Id == riderId).FirstAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
