namespace FoodFlow.Contracts.Restaurants.Dtos
{
    public record RestaurantDetailsResponse(
    int Id,
    string Name,
    string? Description,
    string PhoneNumber,
    string Address,
    string? LogoUrl,
    bool IsOpen);
}

