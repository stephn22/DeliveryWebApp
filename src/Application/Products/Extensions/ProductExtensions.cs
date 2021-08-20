using DeliveryWebApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryWebApp.Application.Products.Extensions
{
    public static class ProductExtensions
    {
        public static decimal DiscountedPrice(this Product product)
        {
            if (product == null)
            {
                return 0.00M;
            }

            if (product.Discount != 0)
            {
                return product.Price * ((100.00M - product.Discount) / 100.00M);
            }

            return product.Price;
        }

        public static decimal TotalPrice(this List<Product> list)
        {
            const decimal tot = 0.00M;
            return list.Count <= 0 ? tot : Math.Round(list.Sum(p => (p.DiscountedPrice() * p.Quantity)), 2);
        }
    }
}
