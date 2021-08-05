using System.Collections.Generic;

namespace DeliveryWebApp.Domain.Entities
{
    public class Restaurateur : Customer
    {
        public byte[] Logo { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantCategory { get; set; }
        public int RestaurantAddressId { get; set; }
        public Address RestaurantAddress { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Order> RestaurantOrders { get; set; }
        public ICollection<Review> RestaurateurReviews { get; set; }
    }
}
