using Microsoft.AspNetCore.Authorization;

namespace FoodFlow.Abstractions.Filters
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
            
    }
}
