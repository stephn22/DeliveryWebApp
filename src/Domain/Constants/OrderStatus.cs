namespace DeliveryWebApp.Domain.Constants
{
    // TODO: New, Checkout, Paid, Failed, Shipped, Delivered, Returned, and Complete ??

    /// <summary>
    /// Describes the status of an order
    /// </summary>
    public static class OrderStatus
    {
        /// <summary>
        /// When the order has just been posted by a customer
        /// </summary>
        public const string New = "New";

        public const string Checkout = "Checkout";
        public const string Taken = "Taken";

        public const string Paid = "Paid";

        public const string Failed = "Failed";

        public const string Shipped = "Shipped";

        public const string Returned = "Returned";

        public const string Complete = "Complete";
        /// <summary>
        /// When the order has been delivered to the customer
        /// </summary>
        public const string Delivered = "Delivered";
    }
}
