using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Request : BaseEntity
    {
        public string Role { get; set; }
        public string Status { get; set; }
        public Client Client { get; set; }
    }
}
