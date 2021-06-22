using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Restaurateur : BaseEntity
    {
        public Client Client { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
