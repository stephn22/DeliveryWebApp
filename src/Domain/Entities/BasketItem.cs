namespace DeliveryWebApp.Domain.Entities
{
    public class BasketItem : BaseEntity
    {
        public int ProductId { get; set; }
        public int BasketId { get; set; }
        public virtual Basket Basket { get; set; }

        /// <summary>
        /// The quantity selected by the customer
        /// </summary>
        public int Quantity { get; set; }
    }
}
