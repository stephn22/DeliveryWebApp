using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace DeliveryWebApp.Infrastructure.Security.Policies
{
    public class IsAuthenticatedAuthorizationHandler : AuthorizationHandler<IsAuthenticated>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAuthenticated requirement)
        {
            if (context.User.IsAuthenticated())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
