using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class Client
    {
        public string ApplicationUserFk { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
