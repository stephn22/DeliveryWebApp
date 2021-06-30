using System.Collections.Generic;

namespace DeliveryWebApp.Domain.Entities
{
    public class Restaurateur : BaseEntity
    {
        public virtual Restaurant Restaurant { get; set; }
        public virtual Customer Customer { get; set; } // a restaurateur is also a customer
        public ICollection<Review> Reviews { get; set; }
    }
}
