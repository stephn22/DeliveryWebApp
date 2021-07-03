using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Infrastructure.Security
{
    public class PolicyName
    {
        /// <summary>
        /// user that manages restaurant and product catalog
        /// </summary>
        public const string IsRestaurateur = "IsRestaurateur";

        /// <summary>
        /// user that deliveries products from restaurant to customer
        /// </summary>
        public const string IsRider = "IsRider";

        /// <summary>
        /// Default user, this entity is neither rider or restaurateur
        /// </summary>
        public const string IsDefault = "IsDefault";

        /// <summary>
        /// User that is a customer, it could be rider or restaurateur
        /// </summary>
        public const string IsCustomer = "IsCustomer";
    }
}
