using DeliveryWebApp.Domain.Entities;

namespace DeliveryWebApp.Domain.Objects
{
    public class AddToBasket
    {
        public int CustomerId { get; set; }
        public Product Product { get; set; }
    }
}
