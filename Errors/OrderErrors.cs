namespace FoodFlow.Errors
{
    public static class OrderErrors
    {
        public static readonly Error NotFound = new("Order.NotFound", "The order was not found.", StatusCodes.Status404NotFound);
        public static readonly Error RestaurantNotFound = new("Order.RestaurantNotFound", "The restaurant was not found.", StatusCodes.Status404NotFound);
        public static readonly Error InvalidMenuItems = new("Order.InvalidMenuItems", "One or more menu items are invalid.", StatusCodes.Status400BadRequest);
        public static readonly Error FailedToCreate = new("Order.FailedToCreate", "Failed to create the order.", StatusCodes.Status500InternalServerError);
        public static readonly Error Unauthorized = new("Order.Unauthorized", "You are not authorized to access this order.", StatusCodes.Status401Unauthorized);
        public static readonly Error NoOrdersFound = new("Order.NoOrdersFound", "No orders found.", StatusCodes.Status404NotFound);
        public static readonly Error CannotCancel = new("Order.CannotCancel", "Cannot cancel this order.", StatusCodes.Status400BadRequest);
    }
}