﻿using System;
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
            return await (from r in context.Restaurateurs
                where r.Id == restaurateurId
                select r.Restaurant).FirstAsync();
        }
    }
}