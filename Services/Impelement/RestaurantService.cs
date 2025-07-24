using Microsoft.Extensions.Caching.Hybrid;

namespace FoodFlow.Services.Impelement
{
    public class RestaurantService(ApplicationDbContext DbContext, HybridCache hybridCache /*ICacheService cacheService*/) : IRestaurantService
    {
        private readonly ApplicationDbContext _dbContext = DbContext;
        private readonly HybridCache _hybridCache = hybridCache;
        #region distributed cache    
        // private readonly ICacheService _cacheService = cacheService;     "distributed cache service"
        #endregion
        private const string _CacheKeyPrefix = "restaurants";
        public async Task<Result<IEnumerable<RestaurantSummaryResponse>>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{_CacheKeyPrefix}_All";
            var restaurantLists = await _hybridCache.GetOrCreateAsync<IEnumerable<RestaurantSummaryResponse>>(
                cacheKey,
                async entry =>
                await _dbContext.Restaurants
                .ProjectToType<RestaurantSummaryResponse>()
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                );
            return Result.Success(restaurantLists);
        }
        public async Task<Result<IEnumerable<RestaurantSummaryResponse>>> GetActiveRestaurantsAsync(CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{_CacheKeyPrefix}_Active";
            var restaurantLists = await _hybridCache.GetOrCreateAsync<IEnumerable<RestaurantSummaryResponse>>(
                cacheKey,
                async entry => await _dbContext.Restaurants
                .Where(x => x.IsActive)
                .ProjectToType<RestaurantSummaryResponse>()
                .AsNoTracking()
                .ToListAsync(cancellationToken)
            );
            return Result.Success(restaurantLists);
        }

        public async Task<Result<RestaurantResponse>> GetRestaurantByIdAsync(int restaurantId, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{_CacheKeyPrefix}_{restaurantId}";

            var existingRestaurant = await _hybridCache.GetOrCreateAsync(
                cacheKey, async entry =>
                await _dbContext.Restaurants
                .Where(x => x.Id == restaurantId)
                .ProjectToType<RestaurantResponse>()
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
               ? Result.Failure<RestaurantResponse>(RestaurantErrors.NotFound)
               : Result.Success(existingRestaurant);
        }

        public async Task<Result<RestaurantResponse>> CreateRestaurantAsync(CreateRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            var restaurantIsExists = await _dbContext.Restaurants
                .AnyAsync(x => (x.Name == request.Name && x.Address == request.Address) || x.PhoneNumber == request.PhoneNumber, cancellationToken);
            if (restaurantIsExists)
                return Result.Failure<RestaurantResponse>(RestaurantErrors.AlreadyExists);

            var newRestaurant = request.Adapt<Restaurant>();
            try
            {
                await _dbContext.Restaurants.AddAsync(newRestaurant, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await InvalidateRestaurantCache(null);

            }
            catch (Exception)
            {
                return Result.Failure<RestaurantResponse>(RestaurantErrors.FailedToCreate);
            }


            var response = newRestaurant.Adapt<RestaurantResponse>();

            return Result.Success(response);
        }
        public async Task<Result> UpdateRestaurantAsync(int restaurantId, UpdateRestaurantRequest request, CancellationToken cancellationToken)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync(restaurantId, cancellationToken);
            if (restaurant is null)
                return Result.Failure(RestaurantErrors.NotFound);

            var restaurantIsExists = await _dbContext.Restaurants
                .AnyAsync(x => x.Id != restaurantId && (
                (x.Name == request.Name && x.Address == request.Address) || x.PhoneNumber == request.PhoneNumber),
                cancellationToken);
            if (restaurantIsExists)
                return Result.Failure(RestaurantErrors.AlreadyExists);

            request.Adapt(restaurant);
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                //await _cacheService.RemoveAsync($"{_CacheKey}_{id}", cancellationToken);       "distributed cache service"
                await InvalidateRestaurantCache(restaurantId);
            }
            catch (Exception)
            {
                return Result.Failure(RestaurantErrors.FailedToUpdate);
            }


            return Result.Success();
        }

        public async Task<Result> DeleteRestaurantAsync(int restaurantId, CancellationToken cancellationToken = default)
        {
            var existingRestaurant = await _dbContext.Restaurants.FindAsync(restaurantId, cancellationToken);
            if (existingRestaurant is null)
                return Result.Failure(RestaurantErrors.NotFound);


            _dbContext.Restaurants.Remove(existingRestaurant);
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                //await _cacheService.RemoveAsync($"{_CacheKey}_{id}", cancellationToken);      "distributed cache service"
                await InvalidateRestaurantCache(restaurantId);
            }
            catch (Exception)
            {
                return Result.Failure(RestaurantErrors.FailedToDelete);
            }
            return Result.Success();
        }

        public async Task<Result> ToggleOpenStatusAsync(int restaurantId, CancellationToken cancellationToken = default)
        {
            var existingRestaurant = await _dbContext.Restaurants.FindAsync(restaurantId, cancellationToken);

            if (existingRestaurant is null)
                return Result.Failure(RestaurantErrors.NotFound);

            existingRestaurant.IsOpen = !existingRestaurant.IsOpen;
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                //await _cacheService.RemoveAsync($"{_CacheKey}_{id}", cancellationToken);      "distributed cache service"
                await InvalidateRestaurantCache(restaurantId);
            }
            catch (Exception)
            {
                return Result.Failure(RestaurantErrors.FailedToToggleStatus);
            }
            return Result.Success();
        }

        public async Task<Result> ToggleActiveStatusAsync(int restaurantId, CancellationToken cancellationToken = default)
        {
            var existingRestaurant = await _dbContext.Restaurants.FindAsync(restaurantId, cancellationToken);

            if (existingRestaurant is null)
                return Result.Failure(RestaurantErrors.NotFound);

            existingRestaurant.IsActive = !existingRestaurant.IsActive;
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                #region  distributed cache service
                //await _cacheService.RemoveAsync($"{_CacheKey}_{id}", cancellationToken); 
                #endregion
                await InvalidateRestaurantCache(restaurantId);
            }
            catch (Exception)
            {
                return Result.Failure(RestaurantErrors.FailedToToggleStatus);
            }
            return Result.Success();
        }


        private async Task InvalidateRestaurantCache(int? restaurantId)
        {
            await _hybridCache.RemoveAsync($"{_CacheKeyPrefix}_All");
            await _hybridCache.RemoveAsync($"{_CacheKeyPrefix}_Active");
            if (restaurantId is null)
                await _hybridCache.RemoveAsync($"{_CacheKeyPrefix}_{restaurantId}");
        }
    }
}
