namespace FoodFlow.Const
{
    public static class RestaurantErrors
    {
        public static readonly Error NotFound = new("Restaurant.NotFound", "The requested restaurant was not found.");
        public static readonly Error FailedToCreate = new("Restaurant.FailedToCreate", "Failed to create the restaurant.");
        public static readonly Error FailedToUpdate = new("Restaurant.FailedToUpdate", "Failed to update the restaurant.");
        public static readonly Error FailedToDelete = new("Restaurant.FailedToDelete", "Failed to delete the restaurant.");
        public static readonly Error FailedToToggleStatus = new("Restaurant.FailedToToggleStatus", "Failed to change the restaurant status.");
        public static readonly Error NoRestaurantsFound = new("Restaurant.NoRestaurantsFound", "No restaurants found.");
    }
}