namespace FoodFlow.Contracts.Restaurants
{
    public record CreateRestaurantRequest(
    string Name,
    string? Description,
    string PhoneNumber,
    string Address,
    string? LogoUrl,
    bool IsOpen);
}