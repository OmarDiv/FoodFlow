
using Microsoft.AspNetCore.Http.HttpResults;

namespace FoodFlow.Services.Impelement
{
    public class RestaurantServicecs(ApplicationDbContext DbContext) : IRestaurantService
    {
        public readonly ApplicationDbContext _DbContext = DbContext;
        public async Task<IEnumerable<RestaurantListResponse>> GetAllRestaurantsAsync()
        {
            var restaurants = await _DbContext.Restaurants.AsNoTracking().ToListAsync();
            var result = restaurants.Adapt<IEnumerable<RestaurantListResponse>>();
            return result;
        }

        public async Task<RestaurantDetailsResponse> GetRestaurntByIdAsync(int id)
        {
            var existingRestaurant = await _DbContext.Restaurants.FindAsync(id);
            if (existingRestaurant is null)
                return null!;
            var result = existingRestaurant.Adapt<RestaurantDetailsResponse>();
            return result;

        }

        public async Task<RestaurantDetailsResponse> CreateRestaurantAsync(CreateRestaurantRequest request)
        {
            var newRestaurant = request.Adapt<Restaurant>();

            var result = await _DbContext.Restaurants.AddAsync(newRestaurant);

            await _DbContext.SaveChangesAsync();

            return result.Adapt<RestaurantDetailsResponse>();
        }

        public async Task<bool> UpdateRestaurantAsync(int id, UpdateRestaurantRequest restaurant)
        {
            var existingRestaurant = await _DbContext.Restaurants.FindAsync(id);
            if (existingRestaurant is null)
                return false;

            var result = restaurant.Adapt(existingRestaurant);
            await _DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteRestaurantAsync(int id)
        {
            var existingRestaurant = await _DbContext.Restaurants.FindAsync(id);
            if (existingRestaurant is null)
                return false;
             _DbContext.Restaurants.Remove(existingRestaurant);
            await _DbContext.SaveChangesAsync();
            return true;

        }
    }
}
