namespace FoodFlow.Contracts.Restaurants.Dtos
{
    public record RestaurantListResponse(
     int Id,
     string Name,
     string Description,
     string LogoUrl,
     bool IsOpen);
}