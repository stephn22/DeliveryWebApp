using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Order : BaseEntity
    {
        public ICollection<Product> Products { get; set; }
        public DateTime Date { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
