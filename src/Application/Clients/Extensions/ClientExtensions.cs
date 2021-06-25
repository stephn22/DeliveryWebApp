using System;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Application.Clients.Extensions
{
    public static class ClientExtensions
    {
        /// <summary>
        /// Get the Client instance given the identifier (Client.Id)
        /// </summary>
        /// <param name="id">Identifier of the client</param>
        /// <returns>Client instance</returns>
        public static async Task<Client> GetClientByRequestIdAsync(this IApplicationDbContext context, int? requestId)
        {
            try
            {
                if (requestId == null)
                {
                    throw new NullReferenceException();
                }

                return await (from request in context.Requests
                    where request.Id == requestId
                    select request.Client).FirstAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static async Task<Client> GetClientByIdAsync(this IApplicationDbContext context, int? clientId)
        {
            try
            {
                if (clientId == null)
                {
                    throw new NullReferenceException();
                }

                return await context.Clients.Where(c => c.Id == clientId).FirstAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

}
