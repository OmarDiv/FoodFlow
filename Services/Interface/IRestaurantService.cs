namespace FoodFlow.Interface
{
    public interface IRestaurantService
    {
        Task<Result<IEnumerable<RestaurantSummaryResponse>>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<RestaurantSummaryResponse>>> GetActiveRestaurantsAsync(CancellationToken cancellationToken = default);
        Task<Result<RestaurantResponse>> GetRestaurantByIdAsync(int restaurantId, CancellationToken cancellationToken = default);
        Task<Result<RestaurantResponse>> CreateRestaurantAsync(CreateRestaurantRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateRestaurantAsync(int restaurantId, UpdateRestaurantRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteRestaurantAsync(int restaurantId, CancellationToken cancellationToken = default);
        Task<Result> ToggleOpenStatusAsync(int restaurantId, CancellationToken cancellationToken = default);
        Task<Result> ToggleActiveStatusAsync(int restaurantId, CancellationToken cancellationToken = default);

    }
}