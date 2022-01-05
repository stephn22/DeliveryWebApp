using System.Collections.Generic;

namespace DeliveryWebApp.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string ApplicationUserFk { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
