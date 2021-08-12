namespace DeliveryWebApp.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string AddressLine { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int? RestaurateurId { get; set; }
        public Restaurateur Restaurateur { get; set; }

        public override string ToString()
        {
            return AddressLine;
        }
    }
}