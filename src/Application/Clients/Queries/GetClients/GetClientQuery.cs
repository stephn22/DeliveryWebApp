using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.Application.Clients.Queries.GetClients
{
    public static class GetClientQuery
    {
        /// <summary>
        /// Get the Client instance given the identifier (Client.Id)
        /// </summary>
        /// <param name="id">Identifier of the client</param>
        /// <returns>Client instance</returns>
        public static async Task<Client> GetClientAsync(this IApplicationDbContext context, int? clientId)
        {
            try
            {
                if (clientId != null)
                {
                    return await (context.Clients.Where(c => c.Id == clientId)).FirstAsync();
                }

                throw new InvalidOperationException();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }

}
