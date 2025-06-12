namespace FoodFlow.Contracts.Restaurants
{
    public record RestaurantListResponse(
     int Id,
     string Name,
     string Description,
     string LogoUrl,
     bool IsOpen);
}