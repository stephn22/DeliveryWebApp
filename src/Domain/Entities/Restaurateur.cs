using System.Collections.Generic;

namespace DeliveryWebApp.Domain.Entities
{
    public class Restaurateur : Customer
    {
        public virtual Restaurant Restaurant { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
