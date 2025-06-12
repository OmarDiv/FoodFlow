using FoodFlow.Contracts.MenuItems;

namespace FoodFlow.Contracts.Categories
{
    public record CategoryResponse(
        int Id,
        string Name,
        int RestaurantId,
        IReadOnlyList<MenuItemResponse> MenuItems
    );

}
