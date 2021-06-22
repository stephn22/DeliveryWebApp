using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Client Client { get; set; }
        public ICollection<Product> Products { get; set; }
        public DateTime Date { get; set; }
        public Restaurant Restaurant { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
