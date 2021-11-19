using System.Collections.Generic;

namespace DeliveryWebApp.Domain.Entities
{
    public class Basket : BaseEntity
    {
        public decimal TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<BasketItem> BasketItems { get; set; }
    }
}
