using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public double ApplyDiscount()
        {
            if (Discount != 0)
            {
                return Price * ((100.00 - Discount) / 100.00);
            }

            return Price;
        }

        public static double TotalPrice(List<Product> list)
        {
            const double tot = 0.00;
            return list.Count <= 0 ? tot : Math.Round(list.Sum(p => (p.ApplyDiscount() * p.Quantity)), 2);
        }
    }
}
