using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryWebApp.Infrastructure.Security.Policies
{
    public class IsAdminAuthorizationHandler : AuthorizationHandler<IsAdmin>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdmin requirement)
        {
            if (context.User.HasClaim(ClaimName.Role, RoleName.Admin))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
