using DeliveryWebApp.Application.Common.Security;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace DeliveryWebApp.Infrastructure.Security.Policies;

public class IsDefaultAuthorizationHandler : AuthorizationHandler<IsDefault>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsDefault requirement)
    {
        if (context.User.HasClaim(ClaimName.Role, RoleName.Default))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}