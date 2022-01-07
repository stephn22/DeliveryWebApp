using System.Collections.Generic;

namespace DeliveryWebApp.Domain.Entities;

public class Restaurateur : BaseEntity
{
    public byte[] Logo { get; set; }
    public string RestaurantName { get; set; }
    public string RestaurantCategory { get; set; }
    public int RestaurantAddressId { get; set; }
    public virtual Address RestaurantAddress { get; set; }
    public virtual ICollection<Product> Products { get; set; }
    public virtual ICollection<Order> RestaurantOrders { get; set; }
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; } // a restaurateur is also a customer
    public virtual ICollection<Review> RestaurateurReviews { get; set; }
}