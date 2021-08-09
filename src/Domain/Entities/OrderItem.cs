using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        //public Product Product { get; set; }

        /// <summary>
        /// The price of the product while purchasing it
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// The discount of the product while purchasing it
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// The quantity of the product selected by the user
        /// </summary>
        public int Quantity { get; set; }
    }
}
