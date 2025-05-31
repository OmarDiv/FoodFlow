namespace FoodFlow.Contracts.MenuItems.Dtos
{
    public record MenuItemDto(
     int Id,
     string Name,
     string Description,
     string LogoUrl,
     bool IsOpen);
}