namespace DeliveryWebApp.Domain.Entities;

public class BasketItem : BaseEntity
{
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    public int BasketId { get; set; }
    public virtual Basket Basket { get; set; }
    public int Quantity { get; set; }
}