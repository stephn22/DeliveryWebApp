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

                return await context.Riders.Where(r => r.Id == riderId).FirstAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task<Rider> GetRiderByClientIdAsync(this IApplicationDbContext context, int? cliendId)
        {
            try
            {
                if (cliendId == null)
                {
                    throw new NullReferenceException();
                }

                return await context.Riders.Where(r => r.Client.Id == cliendId).FirstAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
