using System.Collections.Generic;

namespace DeliveryWebApp.Domain.Entities
{
    public class Rider : BaseEntity
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } // a rider is also a customer
        public double DeliveryCredit { get; set; }
        public ICollection<Order> OpenOrders { get; set; }
    }
}
