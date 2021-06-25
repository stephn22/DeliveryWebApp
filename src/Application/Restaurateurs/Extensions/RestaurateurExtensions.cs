using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Application.Restaurateurs.Extensions
{
    public static class RestaurateurExtensions
    {
        public static async Task<Restaurateur> GetRestaurateurByIdAsync(this IApplicationDbContext context,
            int? restaurateurId)
        {
            try
            {
                if (restaurateurId == null)
                {
                    throw new NullReferenceException();
                }

                return await context.Restaurateurs.Where(r => r.Id == restaurateurId).FirstAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task<Restaurateur> GetRestaurateurByClientIdAsync(this IApplicationDbContext context,
            int? clientId)
        {
            try
            {
                if (clientId == null)
                {
                    throw new NullReferenceException();
                }

                return await context.Restaurateurs.Where(r => r.Client.Id == clientId).FirstAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
