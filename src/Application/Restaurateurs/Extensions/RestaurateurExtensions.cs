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

        public static async Task<Restaurateur> GetRestaurateurByCustomerIdAsync(this IApplicationDbContext context,
            int? customerId)
        {
            try
            {
                if (customerId == null)
                {
                    throw new NullReferenceException();
                }

                return await context.Restaurateurs.Where(r => r.Customer.Id == customerId).FirstAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task<Restaurateur> GetRestaurateurByApplicationUserFkAsync(
            this IApplicationDbContext context, string applicationUserFk)
        {
            var restaurateur = await (from r in context.Restaurateurs
                where r.Customer.ApplicationUserFk == applicationUserFk
                select r).FirstAsync();

            return restaurateur;
        }
    }
}
