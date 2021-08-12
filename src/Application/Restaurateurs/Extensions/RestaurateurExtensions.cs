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
        public static async Task<Restaurateur> GetRestaurateurAsync(this IApplicationDbContext context,
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
            catch (NullReferenceException)
            {
                return null;
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
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static async Task<Restaurateur> GetRestaurateurByApplicationUserFkAsync(
            this IApplicationDbContext context, string applicationUserFk)
        {
            try
            {
                var customer = await context.Customers.FirstAsync(c => c.ApplicationUserFk == applicationUserFk);
                var restaurateur = await context.Restaurateurs.FirstAsync(r => r.CustomerId == customer.Id);

                return restaurateur;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
