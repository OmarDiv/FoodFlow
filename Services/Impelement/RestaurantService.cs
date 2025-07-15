namespace FoodFlow.Services.Impelement
{
    public class RestaurantService(ApplicationDbContext DbContext) : IRestaurantService
    {
        private readonly ApplicationDbContext _dbContext = DbContext;
        public async Task<Result<IEnumerable<RestaurantListResponse>>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default)
        {
            var restaurantLists = await _dbContext.Restaurants
                .AsNoTracking()
                .ProjectToType<RestaurantListResponse>()
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<RestaurantListResponse>>(restaurantLists);
        }
        public async Task<Result<IEnumerable<RestaurantListResponse>>> GetActiveRestaurantsAsync(CancellationToken cancellationToken = default)
        {
            var restaurantLists = await _dbContext.Restaurants
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ProjectToType<RestaurantListResponse>()
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<RestaurantListResponse>>(restaurantLists);
        }

        public async Task<Result<RestaurantDetailsResponse>> GetRestaurantByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingRestaurant =
                 await _dbContext.Restaurants.Where(x => x.Id == id).ProjectToType<RestaurantDetailsResponse>().FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return existingRestaurant is null
                ? Result.Failure<RestaurantDetailsResponse>(RestaurantErrors.NotFound)
                : Result.Success(existingRestaurant);
        }

        public async Task<Result<RestaurantDetailsResponse>> CreateRestaurantAsync(CreateRestaurantRequest request, CancellationToken cancellationToken)
        {
            var restaurantIsExists = await _dbContext.Restaurants
                .AnyAsync(x => x.Name.Trim().ToUpperInvariant() == request.Name.Trim().ToUpperInvariant(), cancellationToken);

            if (restaurantIsExists)
                return Result.Failure<RestaurantDetailsResponse>(RestaurantErrors.AlreadyExists);

            var newRestaurant = request.Adapt<Restaurant>();
            await _dbContext.Restaurants.AddAsync(newRestaurant, cancellationToken);

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                return Result.Failure<RestaurantDetailsResponse>(RestaurantErrors.FailedToCreate);
            }

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

            }
            catch (Exception)
            {
                return Result.Failure(RestaurantErrors.FailedToToggleStatus);
            }
            return Result.Success();
        }


    }
}
