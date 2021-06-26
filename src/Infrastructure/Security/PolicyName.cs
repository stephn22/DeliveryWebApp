using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Infrastructure.Security
{
    public class PolicyName
    {
        public const string IsRestaurateur = "IsRestaurateur";
        public const string IsRider = "IsRider";
        public const string IsDefault = "IsDefault";
        public const string IsEnabled = "IsEnabled"; // user that can be Restaurateur or Rider or Default user but not Admin and is not blocked
    }
}
