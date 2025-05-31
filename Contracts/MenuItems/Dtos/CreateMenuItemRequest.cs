namespace FoodFlow.Contracts.MenuItems.Dtos
{
    public record CreateMenuItemRequest(
     int Id,
     string Name,
     string? Description,
     string Adress,
     string? LogoUrl,
     bool IsOpen);
}