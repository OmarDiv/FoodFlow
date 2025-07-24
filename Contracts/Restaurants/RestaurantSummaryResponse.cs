namespace FoodFlow.Contracts.Restaurants
{
    public record RestaurantSummaryResponse(
     int Id,
     string Name,
     string Description,
     string LogoUrl,
     bool IsOpen);
}