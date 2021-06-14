using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryWebApp.Infrastructure.Security.Policies
{
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
}
