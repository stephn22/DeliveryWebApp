using System.Collections.Generic;

namespace DeliveryWebApp.Domain.Entities
{
    public class Restaurateur : BaseEntity
    {
        public virtual Client Client { get; set; } // a restaurateur is also a client
        public virtual Restaurant Restaurant { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
