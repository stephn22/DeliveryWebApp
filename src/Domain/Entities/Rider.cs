using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Rider
    {
        public string ApplicationUserFk { get; set; }
        public double DeliveryCredit { get; set; }
        public ICollection<Order> OpenOrders { get; set; }
    }
}
