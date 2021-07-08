namespace DeliveryWebApp.Domain.Constants
{
    /// <summary>
    /// Describes the status of a user request for changing role
    /// </summary>
    public static class RequestStatus
    {
        /// <summary>
        /// The user request has been accepted by the admistrator
        /// </summary>
        public const string Accepted = "Accepted";

        /// <summary>
        /// Request hasn't been accepted by the administrator yet
        /// </summary>
        public const string Idle = "Idle";

        /// <summary>
        /// Request has been rejected by administrator
        /// </summary>
        public const string Rejected = "Rejected";
    }
}