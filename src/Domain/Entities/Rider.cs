using System.Collections.Generic;

namespace DeliveryWebApp.Domain.Entities
{
    public class Rider : Customer
    {
        public decimal DeliveryCredit { get; set; }
        public ICollection<Order> OpenOrders { get; set; }
    }
}
