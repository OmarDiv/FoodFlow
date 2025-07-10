namespace FoodFlow.Const
{
    public static class MenuItemErrors
    {
        public static readonly Error NotFound = new("MenuItem.NotFound", "The requested menu item was not found.");
        public static readonly Error NoItemsFound = new("MenuItem.NoItemsFound", "No menu items found for the specified category and restaurant.");
        public static readonly Error AlreadyExists = new("MenuItem.AlreadyExists", "A menu item with the same name already exists in this category.");
        public static readonly Error FailedToCreate = new("MenuItem.FailedToCreate", "Failed to create the menu item.");
        public static readonly Error FailedToUpdate = new("MenuItem.FailedToUpdate", "Failed to update the menu item.");
        public static readonly Error FailedToDelete = new("MenuItem.FailedToDelete", "Failed to delete the menu item.");
        public static readonly Error FailedToToggleAvailability = new("MenuItem.FailedToToggleAvailability", "Failed to change the availability status of the menu item.");
        public static readonly Error InvalidRequest = new("MenuItem.InvalidRequest", "The menu item request is invalid.");
    }
}
