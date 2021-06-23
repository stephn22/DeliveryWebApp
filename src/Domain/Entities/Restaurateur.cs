using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Restaurateur : BaseEntity
    {
        public virtual Client Client { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
