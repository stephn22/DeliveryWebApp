using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string ApplicationUserFk { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public virtual Basket Basket { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
