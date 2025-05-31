namespace FoodFlow.Contracts.Restaurants.Dtos
{
    public record UpdateRestaurantRequest(
     string Name,
     string? Description,
     int PhoneNumber,
     string Address,
     string? LogoUrl,
     bool IsOpen);
}