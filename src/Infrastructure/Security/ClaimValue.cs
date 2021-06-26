using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Infrastructure.Security
{
    public static class ClaimValue
    {
        public const string Enabled = "Enabled"; // user can login and use website
        public const string Disabled = "Disabled"; // user is blocked
    }
}
