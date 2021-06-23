using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Basket : BaseEntity
    {
        public ICollection<Product> Products { get; set; }

        public double TotalPrice { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
