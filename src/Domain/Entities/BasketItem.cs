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
        public int BasketId { get; set; }
        public Basket Basket { get; set; }

        /// <summary>
        /// The quantity of the product selected by the customer
        /// </summary>
        public int Quantity { get; set; }
    }
}
