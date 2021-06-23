using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Restaurant : BaseEntity
    {
        public string LogoUrl { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public virtual Address Address { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }

        public int? RestaurateurId { get; set; }
        public virtual Restaurateur Restaurateur { get; set; }
    }
}
