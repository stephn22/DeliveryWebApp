using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryWebApp.Infrastructure.Security.Policies
{
    public class IsRestaurateurAuthorizationHandler : AuthorizationHandler<IsRestaurateur>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsRestaurateur requirement)
        {
            if (context.User.HasClaim(ClaimName.Role, RoleName.Restaurateur))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
