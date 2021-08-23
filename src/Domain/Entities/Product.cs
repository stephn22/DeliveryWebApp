namespace DeliveryWebApp.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public int RestaurateurId { get; set; }
        public Restaurateur Restaurateur { get; set; }
    }
}
