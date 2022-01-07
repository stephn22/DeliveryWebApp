namespace DeliveryWebApp.Domain.Entities;

public class Rider : BaseEntity
{
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; } // a rider is also a customer
    public decimal DeliveryCredit { get; set; }
    public decimal TotalCredit { get; set; }
}