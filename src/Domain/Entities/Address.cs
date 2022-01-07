namespace DeliveryWebApp.Domain.Entities;

public class Address : BaseEntity
{
    public string PlaceName { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public int? CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    public int? RestaurateurId { get; set; }
    public virtual Restaurateur Restaurateur { get; set; }
}