using System.Collections.Generic;

namespace FoodFlow.Services.Impelement
{
    public class RestaurantService(ApplicationDbContext DbContext) : IRestaurantService
    {
        private readonly ApplicationDbContext _dbContext = DbContext;
        public async Task<Result<IEnumerable<RestaurantListResponse>>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default)
        {
            var list = await _dbContext.Restaurants
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ProjectToType<RestaurantListResponse>()
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<RestaurantListResponse>>(list);
        }

        public async Task<Result<RestaurantDetailsResponse>> GetRestaurantByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingRestaurant = await _dbContext.Restaurants.FindAsync(id, cancellationToken);
            if (existingRestaurant is null)
                return Result.Failure<RestaurantDetailsResponse>(RestaurantErrors.NotFound);
            var result = existingRestaurant.Adapt<RestaurantDetailsResponse>();

            return Result.Success(result);

        }

        public async Task<Result<RestaurantDetailsResponse>> CreateRestaurantAsync(CreateRestaurantRequest request, CancellationToken cancellationToken)
        {
            var newRestaurant = request.Adapt<Restaurant>();
            var result = await _dbContext.Restaurants.AddAsync(newRestaurant, cancellationToken);
            if (result.Entity is null)
                return Result.Failure<RestaurantDetailsResponse>(RestaurantErrors.FailedToCreate);
            await _dbContext.SaveChangesAsync(cancellationToken);
            var response = result.Entity.Adapt<RestaurantDetailsResponse>();

            return Result.Success(response);
        }

        public async Task<Result> UpdateRestaurantAsync(int id, UpdateRestaurantRequest restaurant, CancellationToken cancellationToken)
        {
            var existingRestaurant = await _dbContext.Restaurants.FindAsync(id, cancellationToken);
            if (existingRestaurant is null)
                return Result.Failure(RestaurantErrors.NotFound);

            restaurant.Adapt(existingRestaurant);

            var saveResult = await _dbContext.SaveChangesAsync(cancellationToken);
            if (saveResult == 0)
                return Result.Failure(RestaurantErrors.FailedToUpdate);

            return Result.Success();
        }

        public async Task<Result> DeleteRestaurantAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingRestaurant = await _dbContext.Restaurants.FindAsync(id, cancellationToken);
            if (existingRestaurant is null)
                return Result.Failure(RestaurantErrors.NotFound);

            try
            {
                _dbContext.Restaurants.Remove(existingRestaurant);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch
            {
                return Result.Failure(RestaurantErrors.FailedToDelete);
            }
        }

        public async Task<Result> ToggleOpenStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingRestaurant = await _dbContext.Restaurants.FindAsync(id, cancellationToken);

            if (existingRestaurant is null)
                return Result.Failure(RestaurantErrors.NotFound);

            try
            {
                existingRestaurant.IsOpen = !existingRestaurant.IsOpen;
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch
            {
                return Result.Failure(RestaurantErrors.FailedToToggleStatus);
            }
        }

        public async Task<Result> ToggleActiveStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingRestaurant = await _dbContext.Restaurants.FindAsync(id, cancellationToken);

            if (existingRestaurant is null)
                return Result.Failure(RestaurantErrors.NotFound);

            try
            {
                existingRestaurant.IsActive = !existingRestaurant.IsActive;
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch
            {
                return Result.Failure(RestaurantErrors.FailedToToggleStatus);
            }
        }

    }
}
