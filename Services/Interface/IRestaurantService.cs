namespace FoodFlow.Interface
{
    public interface IRestaurantService
    {
        Task<IEnumerable<RestaurantListResponse>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default);
        Task<RestaurantDetailsResponse> GetRestaurantByIdAsync(int restaurantId, CancellationToken cancellationToken = default);
        Task<RestaurantDetailsResponse> CreateRestaurantAsync(CreateRestaurantRequest request, CancellationToken cancellationToken = default);
        Task<bool> UpdateRestaurantAsync(int id, UpdateRestaurantRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteRestaurantAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ToggleOpenStatusAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ToggleActiveStatusAsync(int id, CancellationToken cancellationToken = default);

    }
}