namespace FoodFlow.Interface
{
    public interface IRestaurantService
    {
        Task<Result<IEnumerable<RestaurantListResponse>>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<RestaurantListResponse>>> GetActiveRestaurantsAsync(CancellationToken cancellationToken = default);
        Task<Result<RestaurantDetailsResponse>> GetRestaurantByIdAsync(int restaurantId, CancellationToken cancellationToken = default);
        Task<Result<RestaurantDetailsResponse>> CreateRestaurantAsync(CreateRestaurantRequest request, string userId, CancellationToken cancellationToken = default);
        Task<Result> UpdateRestaurantAsync(int id, UpdateRestaurantRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteRestaurantAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> ToggleOpenStatusAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> ToggleActiveStatusAsync(int id, CancellationToken cancellationToken = default);

    }
}