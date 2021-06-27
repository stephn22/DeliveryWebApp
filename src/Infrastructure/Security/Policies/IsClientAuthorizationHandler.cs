using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryWebApp.Infrastructure.Security.Policies
{
    public class IsClientAuthorizationHandler : AuthorizationHandler<IsClient>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsClient requirement)
        {
            if (context.User.HasClaim(ClaimName.Role, RoleName.Restaurateur) ||
                context.User.HasClaim(ClaimName.Role, RoleName.Rider) ||
                context.User.HasClaim(ClaimName.Role, RoleName.Default))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
