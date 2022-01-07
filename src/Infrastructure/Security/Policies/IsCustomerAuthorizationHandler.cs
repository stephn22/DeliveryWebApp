using DeliveryWebApp.Application.Common.Security;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace DeliveryWebApp.Infrastructure.Security.Policies;

public class IsCustomerAuthorizationHandler : AuthorizationHandler<IsCustomer>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCustomer requirement)
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