using System;
using System.Collections.Generic;

namespace DeliveryWebApp.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public DateTime Date { get; set; }
        public int RestaurateurId { get; set; }
        public virtual Restaurateur Restaurateur { get; set; }
        public int? RiderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int DeliveryAddressId { get; set; }
        public virtual Address DeliveryAddress { get; set; }
    }
}
