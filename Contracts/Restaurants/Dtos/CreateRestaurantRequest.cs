namespace FoodFlow.Contracts.Restaurants.Dtos
{
    public record CreateRestaurantRequest(
    string Name,
    string? Description,
    string PhoneNumber,
    string Address,
    string? LogoUrl,
    bool IsOpen);
}