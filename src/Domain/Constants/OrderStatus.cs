namespace DeliveryWebApp.Domain.Constants;

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
    public const string Paid = "Paid";
    public const string Shipped = "Shipped";

    /// <summary>
    /// Order has been taken by a rider
    /// </summary>
    public const string Taken = "Taken";
    public const string Failed = "Failed";
    public const string Delivered = "Delivered";
    public const string Returned = "Returned";
}