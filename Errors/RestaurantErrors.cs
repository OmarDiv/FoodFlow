namespace FoodFlow.Errors
{
    public static class RestaurantErrors
    {
        public static readonly Error NotFound = new("Restaurant.NotFound", "The requested restaurant was not found.", StatusCodes.Status404NotFound);
        public static readonly Error FailedToCreate = new("Restaurant.FailedToCreate", "Failed to create the restaurant.", StatusCodes.Status500InternalServerError);
        public static readonly Error FailedToUpdate = new("Restaurant.FailedToUpdate", "Failed to update the restaurant.", StatusCodes.Status400BadRequest);
        public static readonly Error FailedToDelete = new("Restaurant.FailedToDelete", "Failed to delete the restaurant.", StatusCodes.Status400BadRequest);
        public static readonly Error FailedToToggleStatus = new("Restaurant.FailedToToggleStatus", "Failed to change the restaurant status.", StatusCodes.Status400BadRequest);
        public static readonly Error AlreadyExists = new("Restaurant.AlreadyExists", "A restaurant with the same Name or Phone Number already exists.", StatusCodes.Status409Conflict);
    }
}