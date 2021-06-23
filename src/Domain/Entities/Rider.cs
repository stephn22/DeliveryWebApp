using System.Collections.Generic;

namespace DeliveryWebApp.Domain.Entities
{
    public class Rider : BaseEntity
    {
        public virtual Client Client { get; set; } // a rider is also a client
        public double DeliveryCredit { get; set; }
        public ICollection<Order> OpenOrders { get; set; }

    }
}
