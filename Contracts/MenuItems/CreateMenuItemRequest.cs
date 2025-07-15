namespace FoodFlow.Contracts.MenuItems;
public record CreateMenuItemRequest(
     string Name,
     string Description,
     decimal Price,
     string? ImageUrl,
     bool IsAvailable
    );
