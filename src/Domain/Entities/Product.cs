using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
    }
}
