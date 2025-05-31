namespace FoodFlow.Contracts.MenuItems.Dtos
{
    public record UpdateMenuItemRequest(
     int Id,
     string Name,
     string Description,
     string LogoUrl,
     bool IsOpen);
}