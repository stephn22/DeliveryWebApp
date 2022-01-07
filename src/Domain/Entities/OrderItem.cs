namespace DeliveryWebApp.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }

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