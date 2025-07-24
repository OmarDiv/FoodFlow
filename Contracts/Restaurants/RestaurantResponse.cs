namespace FoodFlow.Contracts.Restaurants
{
    public record RestaurantResponse(
    int Id,
    string Name,
    string? Description,
    string PhoneNumber,
    string Address,
    string? LogoUrl,
    bool IsOpen);
}

