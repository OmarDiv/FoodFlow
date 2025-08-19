using Microsoft.AspNetCore.Authorization;

namespace FoodFlow.Abstractions.Filters
{
    public class HasPermissionAttribute(string permission) : AuthorizeAttribute( permission)
    {
        
    }
}
