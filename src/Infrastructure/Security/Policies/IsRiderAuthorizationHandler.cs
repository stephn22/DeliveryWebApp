using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryWebApp.Infrastructure.Security.Policies
{
    public class IsRiderAuthorizationHandler : AuthorizationHandler<IsRider>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsRider requirement)
        {
            if (context.User.HasClaim(ClaimName.Role, RoleName.Rider))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
