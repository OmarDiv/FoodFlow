namespace FoodFlow.Contracts.Restaurants.Dtos
{
    public record CreateRestaurantRequest(
     string Name,
     string? Description,
     int PhoneNumber,
     string Address,
     string? LogoUrl,
     bool IsOpen);
}