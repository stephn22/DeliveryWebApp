using Microsoft.AspNetCore.Authorization;

namespace DeliveryWebApp.Infrastructure.Security.Policies;

public class IsAuthenticated : IAuthorizationRequirement
{
}