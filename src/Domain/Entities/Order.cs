using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public ICollection<Product> Products { get; set; }
        public DateTime Date { get; set; }
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}
