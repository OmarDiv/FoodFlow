using FoodFlow.Contracts.Restaurants.Dtos;

namespace FoodFlow.Interface
{
    public interface IRestaurantService
    {
        Task<IEnumerable<RestaurantListResponse>> GetAllRestaurantsAsync();
        Task<RestaurantDetailsResponse> GetRestaurntByIdAsync(int restaurantId);
        Task<RestaurantDetailsResponse> CreateRestaurantAsync(CreateRestaurantRequest request);
        Task<bool> UpdateRestaurantAsync(int id, UpdateRestaurantRequest request);
        Task<bool> DeleteRestaurantAsync(int id);

    }
}