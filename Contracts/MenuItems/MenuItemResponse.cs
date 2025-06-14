namespace FoodFlow.Contracts.MenuItems
{
    public record MenuItemResponse(
        int Id,
        string Name,
        string Description,
        decimal Price,
        string ImageUrl,
        bool IsAvailable);
}