namespace FoodFlow.Contracts.Restaurants.Dtos
{
    public record UpdateRestaurantRequest(
     string Name,
     string? Description,
     string PhoneNumber,
     string Address,
     string? LogoUrl,
     bool IsOpen);
}