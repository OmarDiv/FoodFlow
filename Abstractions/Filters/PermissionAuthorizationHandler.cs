using FoodFlow.Abstractions.Consts;
using Microsoft.AspNetCore.Authorization;

namespace FoodFlow.Abstractions.Filters
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.Identity is not { IsAuthenticated: true }
            || !context.User.Claims.Any(c => c.Type == Permissions.Type && c.Value.Contains(requirement.Permission)))
                return;

            context.Succeed(requirement);
            return;

        }
    }
}
