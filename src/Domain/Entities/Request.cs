namespace DeliveryWebApp.Domain.Entities;

/// <summary>
/// This class represent a request that a customer sends in order to become a rider or a restaurateur (not both)
/// </summary>
public class Request : BaseEntity
{
    /// <summary>
    /// Role requested by customer
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// status of request ("Idle", "Accepted", "Rejected")
    /// </summary>
    public string Status { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; }
}