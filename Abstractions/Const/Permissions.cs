namespace FoodFlow.Abstractions.Consts;
public static class Permissions
{
    public static string Type => "permissions";

    // Restaurants
    public const string ViewRestaurants = "restaurants:read";
    public const string CreateRestaurant = "restaurants:create";
    public const string UpdateRestaurant = "restaurants:update";
    public const string DeleteRestaurant = "restaurants:delete";
    public const string ToggleRestaurantOpen = "restaurants:toggle-open";
    public const string ToggleRestaurantActive = "restaurants:toggle-active";

    // Categories
    public const string ViewCategories = "categories:read";
    public const string CreateCategory = "categories:create";
    public const string UpdateCategory = "categories:update";
    public const string DeleteCategory = "categories:delete";
    public const string ToggleCategoryStatus = "categories:toggle-status";

    // Menu Items
    public const string ViewMenuItems = "menu-items:read";
    public const string CreateMenuItem = "menu-items:create";
    public const string UpdateMenuItem = "menu-items:update";
    public const string DeleteMenuItem = "menu-items:delete";
    public const string ToggleMenuItemAvailability = "menu-items:toggle-availability";

    // Orders
    public const string CreateOrder = "orders:create";
    public const string CancelOrder = "orders:cancel";
    public const string ViewOrder = "orders:view";
    public const string ViewCustomerOrders = "orders:view-customer";
    public const string ViewRestaurantOrders = "orders:view-restaurant";
    public const string UpdateOrderStatus = "orders:update-status";

    // Roles
    public const string ViewRoles = "roles:read";
    public const string CreateRole = "roles:create";
    public const string UpdateRole = "roles:update";
    public const string DeleteRole = "roles:delete";
    public const string AssignPermissionsToRole = "roles:assign-permissions";

    // Promotions
    public const string ViewPromotions = "promotions:read";
    public const string CreatePromotion = "promotions:create";
    public const string UpdatePromotion = "promotions:update";
    public const string DeletePromotion = "promotions:delete";
    public const string TogglePromotionStatus = "promotions:toggle-status";
    public const string AssignPromotionToRestaurant = "promotions:assign-restaurant";

    // Delivery Zones
    public const string ViewDeliveryZones = "delivery-zones:read";
    public const string CreateDeliveryZone = "delivery-zones:create";
    public const string UpdateDeliveryZone = "delivery-zones:update";
    public const string DeleteDeliveryZone = "delivery-zones:delete";

    // Dlivery
    public const string ViewDlivery = "Dlivery:read";
    public const string CreateCourier = "Dlivery:create";
    public const string UpdateCourier = "Dlivery:update";
    public const string DeleteCourier = "Dlivery:delete";

    // Users
    public const string ViewUsers = "users:read";
    public const string CreateUser = "users:create";
    public const string UpdateUser = "users:update";
    public const string AssignRolesToUser = "users:assign-roles";



    public static IList<string?> GetAllPermissions() =>
    typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string).ToList();
}
