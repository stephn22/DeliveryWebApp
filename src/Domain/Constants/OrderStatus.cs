namespace DeliveryWebApp.Domain.Constants
{
    /// <summary>
    /// Describes the status of an order
    /// </summary>
    public static class OrderStatus
    {
        /// <summary>
        /// When the order has just been posted by a customer
        /// </summary>
        public const string Open = "Open";

        /// <summary>
        /// When the order has been delivered to the customer
        /// </summary>
        public const string Delivered = "Delivered";
    }
}
