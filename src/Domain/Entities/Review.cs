using System;

namespace DeliveryWebApp.Domain.Entities
{
    public class Review : BaseEntity
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public int RestaurateurId { get; set; }
        public virtual Restaurateur Restaurateur { get; set; }
    }
}
