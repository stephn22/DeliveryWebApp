using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public class BasketItem : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }

        /// <summary>
        /// Product price while purchasing it
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// The discount of the product while purchasing it
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// The quantity of the product selected by the customer
        /// </summary>
        public int Quantity { get; set; }
    }
}
