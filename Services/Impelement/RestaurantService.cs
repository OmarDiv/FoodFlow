using Microsoft.Extensions.Caching.Hybrid;

namespace FoodFlow.Services.Impelement
{
    public class RestaurantService(ApplicationDbContext DbContext, HybridCache hybridCache /*ICacheService cacheService*/) : IRestaurantService
    {
        private readonly ApplicationDbContext _dbContext = DbContext;
        private readonly HybridCache _hybridCache = hybridCache;

        // private readonly ICacheService _cacheService = cacheService;     "distributed cache service"

        string CacheKey = "RestaurantList";
        private const string _CacheKey = "RestaurantList";

        public async Task<Result<IEnumerable<RestaurantListResponse>>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{_CacheKey}_All";
            var restaurantLists = await _hybridCache.GetOrCreateAsync<IEnumerable<RestaurantListResponse>>(
                cacheKey,
                async entry =>
                await _dbContext.Restaurants
                .ProjectToType<RestaurantListResponse>()
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                );
            return Result.Success(restaurantLists);
        }
        public async Task<Result<IEnumerable<RestaurantListResponse>>> GetActiveRestaurantsAsync(CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{_CacheKey}_Active";
            var restaurantLists = await _hybridCache.GetOrCreateAsync<IEnumerable<RestaurantListResponse>>(
                cacheKey,
                async entry => await _dbContext.Restaurants
                .Where(x => x.IsActive)
                .ProjectToType<RestaurantListResponse>()
                .AsNoTracking()
                .ToListAsync(cancellationToken)
            );
            return Result.Success(restaurantLists);
        }

        public async Task<Result<RestaurantDetailsResponse>> GetRestaurantByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{_CacheKey}_{id}";

            var existingRestaurant = await _hybridCache.GetOrCreateAsync(
                cacheKey, async entry =>
                await _dbContext.Restaurants
                .Where(x => x.Id == id)
                .ProjectToType<RestaurantDetailsResponse>()
                .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                );
            #region distributed cache service

            //:::"distributed cache service"
            ////var cachedRestaurant = await _cacheService.GetAsync<RestaurantDetailsResponse>(cacheKey, cancellationToken);

            //if (cachedRestaurant is not null)
            //{
            //    return Result.Success(cachedRestaurant);                    
            //}
            //await _cacheService.SetAsync(cacheKey, existingRestaurant, cancellationToken);      "distributed cache service" 
            #endregion

            return existingRestaurant is null
               ? Result.Failure<RestaurantDetailsResponse>(RestaurantErrors.NotFound)
               : Result.Success(existingRestaurant);
        }

        public async Task<Result<RestaurantDetailsResponse>> CreateRestaurantAsync(CreateRestaurantRequest request, string user, CancellationToken cancellationToken)
        {
            var restaurantIsExists = await _dbContext.Restaurants
                .AnyAsync(x => x.Name == request.Name, cancellationToken);

            if (restaurantIsExists)
                return Result.Failure<RestaurantDetailsResponse>(RestaurantErrors.AlreadyExists);

            var newRestaurant = request.Adapt<Restaurant>();
            newRestaurant.CreatedById = user;
            await _dbContext.Restaurants.AddAsync(newRestaurant, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = newRestaurant.Adapt<RestaurantDetailsResponse>();

            return Result.Success(response);
        }
        public async Task<Result> UpdateRestaurantAsync(int id, UpdateRestaurantRequest request, CancellationToken cancellationToken)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync(id, cancellationToken);
            if (restaurant is null)
                return Result.Failure(RestaurantErrors.NotFound);

            var restaurantIsExists = await _dbContext.Restaurants
               .AnyAsync(x => x.Id != id && x.Name.Trim().ToUpperInvariant() == request.Name.Trim().ToUpperInvariant(), cancellationToken);
            if (restaurantIsExists)
                return Result.Failure(RestaurantErrors.AlreadyExists);

            request.Adapt(restaurant);
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                //await _cacheService.RemoveAsync($"{_CacheKey}_{id}", cancellationToken);       "distributed cache service"
                await _hybridCache.RemoveAsync($"{_CacheKey}_{id}", cancellationToken);
            }
            catch (Exception)
            {
                return Result.Failure(RestaurantErrors.FailedToUpdate);
            }


            return Result.Success();
        }

        public async Task<Result> DeleteRestaurantAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingRestaurant = await _dbContext.Restaurants.FindAsync(id, cancellationToken);
            if (existingRestaurant is null)
                return Result.Failure(RestaurantErrors.NotFound);


            _dbContext.Restaurants.Remove(existingRestaurant);
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                //await _cacheService.RemoveAsync($"{_CacheKey}_{id}", cancellationToken);      "distributed cache service"
                await _hybridCache.RemoveAsync($"{_CacheKey}_{id}", cancellationToken);
            }
            catch (Exception)
            {
                return Result.Failure(RestaurantErrors.FailedToDelete);
            }
            return Result.Success();
        }

        public async Task<Result> ToggleOpenStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingRestaurant = await _dbContext.Restaurants.FindAsync(id, cancellationToken);

            if (existingRestaurant is null)
                return Result.Failure(RestaurantErrors.NotFound);

            existingRestaurant.IsOpen = !existingRestaurant.IsOpen;
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                //await _cacheService.RemoveAsync($"{_CacheKey}_{id}", cancellationToken);      "distributed cache service"
                await _hybridCache.RemoveAsync($"{_CacheKey}_{id}", cancellationToken);
            }
            catch (Exception)
            {
                return Result.Failure(RestaurantErrors.FailedToToggleStatus);
            }
            return Result.Success();
        }

        public async Task<Result> ToggleActiveStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingRestaurant = await _dbContext.Restaurants.FindAsync(id, cancellationToken);

            if (existingRestaurant is null)
                return Result.Failure(RestaurantErrors.NotFound);

            existingRestaurant.IsActive = !existingRestaurant.IsActive;
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                //await _cacheService.RemoveAsync($"{_CacheKey}_{id}", cancellationToken);       "distributed cache service"
                await _hybridCache.RemoveAsync($"{_CacheKey}_{id}", cancellationToken);
            }
            catch (Exception)
            {
                return Result.Failure(RestaurantErrors.FailedToToggleStatus);
            }
            return Result.Success();
        }

    }
}
