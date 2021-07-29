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
        public decimal Price { get; set; } // FIXME: price value stored in database isn't correct
        public int Discount { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public decimal ApplyDiscount()
        {
            if (Discount != 0)
            {
                return Price * ((100.00M - Discount) / 100.00M);
            }

            return Price;
        }

        public static decimal TotalPrice(List<Product> list)
        {
            const decimal tot = 0.00M;
            return list.Count <= 0 ? tot : Math.Round(list.Sum(p => (p.ApplyDiscount() * p.Quantity)), 2);
        }
    }
}
