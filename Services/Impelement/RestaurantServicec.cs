using FoodFlow.Contracts.Restaurants;

namespace FoodFlow.Services.Impelement
{
    public class RestaurantServicec(ApplicationDbContext DbContext) : IRestaurantService
    {
        public readonly ApplicationDbContext _DbContext = DbContext;
        public async Task<IEnumerable<RestaurantListResponse>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default)
        {
            var restaurants = await _DbContext.Restaurants.AsNoTracking().ToListAsync(cancellationToken);
            var result = restaurants.Adapt<IEnumerable<RestaurantListResponse>>();
            return result;
        }

        public async Task<RestaurantDetailsResponse> GetRestaurantByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingRestaurant = await _DbContext.Restaurants.FindAsync(id, cancellationToken);
            if (existingRestaurant is null)
                return null!;

            var result = existingRestaurant.Adapt<RestaurantDetailsResponse>();

            return result;

        }

        public async Task<RestaurantDetailsResponse> CreateRestaurantAsync(CreateRestaurantRequest request, CancellationToken cancellationToken)
        {
            var newRestaurant = request.Adapt<Restaurant>();

            var result = await _DbContext.Restaurants.AddAsync(newRestaurant, cancellationToken);

            await _DbContext.SaveChangesAsync(cancellationToken);

            return result.Entity.Adapt<RestaurantDetailsResponse>();
        }

        public async Task<bool> UpdateRestaurantAsync(int id, UpdateRestaurantRequest restaurant, CancellationToken cancellationToken)
        {
            var existingRestaurant = await _DbContext.Restaurants.FindAsync(id, cancellationToken);
            if (existingRestaurant is null)
                return false;

            var result = restaurant.Adapt(existingRestaurant);
            await _DbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteRestaurantAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingRestaurant = await _DbContext.Restaurants.FindAsync(id, cancellationToken);
            if (existingRestaurant is null)
                return false;
            _DbContext.Restaurants.Remove(existingRestaurant);
            await _DbContext.SaveChangesAsync(cancellationToken);
            return true;

        }

        public async Task<bool> ToggleOpenStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var ExistedRestaurant = await _DbContext.Restaurants.FindAsync(id, cancellationToken);

            if (ExistedRestaurant is null)
                return false;

            ExistedRestaurant.IsOpen = !ExistedRestaurant.IsOpen;

            await _DbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> ToggleActiveStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var ExistedRestaurant = await _DbContext.Restaurants.FindAsync(id, cancellationToken);

            if (ExistedRestaurant is null)
                return false;

            ExistedRestaurant.IsActive = !ExistedRestaurant.IsActive;

            await _DbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
