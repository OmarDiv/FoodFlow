namespace FoodFlow.Errors
{
    public class RoleErrors
    {
        public static readonly Error NotFound = new("Role.RoleNotFound", "Role Not Found", StatusCodes.Status404NotFound);
        public static readonly Error RoleAlreadyExists = new("Role.RoleAlreadyExists", "Role With The Same Name Already Exists", StatusCodes.Status409Conflict);
        public static readonly Error InvalidPermissions = new("Role.InvalidPermissions", "Invalid Permissions", StatusCodes.Status400BadRequest);

    }
}
