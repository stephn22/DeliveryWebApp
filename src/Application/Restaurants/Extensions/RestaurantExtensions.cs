using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Application.Restaurants.Extensions
{
    public static class RestaurantExtensions
    {
        public static async Task<Restaurant> GetRestaurantByRestaurateurId(this IApplicationDbContext context,
            int restaurateurId)
        {
            try
            {
                return await (from r in context.Restaurants
                    where r.RestaurateurId == restaurateurId
                    select r).FirstAsync();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public static async Task<Address> GetRestaurantAddress(this IApplicationDbContext context,
            Restaurant restaurant)
        {
            try
            {
                return await (from a in context.Addresses
                    where a.Id == restaurant.AddressId
                    select a).FirstAsync();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
        
    }
}
