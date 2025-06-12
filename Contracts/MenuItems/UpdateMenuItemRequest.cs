namespace FoodFlow.Contracts.MenuItems
{
    public record UpdateMenuItemRequest(
     string Name,
     string Description,
     int Price,
     string ImageUrl,
     bool IsAvailable,
     int CategoryId);
}