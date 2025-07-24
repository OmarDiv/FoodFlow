using FoodFlow.Contracts.Categories;
using Microsoft.Extensions.Caching.Hybrid;

namespace FoodFlow.Services.Impelement
{
    public class CategoryService(ApplicationDbContext applicationDbContext, HybridCache hybridCache) : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext = applicationDbContext;
        private readonly HybridCache _hybridCache = hybridCache;
        private const string _CacheKeyPrefix = "categories";

        public async Task<Result<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync(int restaurantId, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{_CacheKeyPrefix}_{restaurantId}_ALL";

            var existRestarant = await _dbContext.Restaurants.AnyAsync(r => r.Id == restaurantId, cancellationToken);
            if (!existRestarant)
                return Result.Failure<IEnumerable<CategoryResponse>>(RestaurantErrors.NotFound);
            var categories = await _hybridCache.GetOrCreateAsync<IEnumerable<CategoryResponse>>(
                cacheKey,
                async entry => await _dbContext.Categories
                      .Where(c => c.RestaurantId == restaurantId)
                      .ProjectToType<CategoryResponse>()
                      .AsNoTracking()
                      .ToListAsync(cancellationToken)
            );

            return Result.Success(categories);
        }
        public async Task<Result<IEnumerable<CategoryResponse>>> GetAvailableCategoriesAsync(int restaurantId, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{_CacheKeyPrefix}_{restaurantId}_Available";

            var existRestarant = await _dbContext.Restaurants.AnyAsync(r => r.Id == restaurantId, cancellationToken);
            if (!existRestarant)
                return Result.Failure<IEnumerable<CategoryResponse>>(RestaurantErrors.NotFound);
            var categories = await _hybridCache.GetOrCreateAsync<IEnumerable<CategoryResponse>>(
                cacheKey,
                async entry => await _dbContext.Categories
                      .Where(c => c.RestaurantId == restaurantId && c.IsAvailable == true)
                      .ProjectToType<CategoryResponse>()
                      .AsNoTracking()
                      .ToListAsync(cancellationToken)
            );

            return Result.Success(categories);
        }

        public async Task<Result<CategoryResponse>> GetCategoryByIdAsync(int restaurantId, int categoryId, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{_CacheKeyPrefix}_{restaurantId}_{categoryId}";
            var existRestarant = await _dbContext.Categories.AnyAsync(c => c.RestaurantId == restaurantId && c.Id == categoryId, cancellationToken);
            if (!existRestarant)
                return Result.Failure<CategoryResponse>(RestaurantErrors.NotFound);

            var category = await _hybridCache.GetOrCreateAsync(
                cacheKey,
                async entry =>
                 await _dbContext.Categories
                .Where(c => c.RestaurantId == restaurantId && c.Id == categoryId)
                .ProjectToType<CategoryResponse>()
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken)
            );

            return category is null
                 ? Result.Failure<CategoryResponse>(CategoryErrors.NotFound)
                 : Result.Success(category);
        }

        public async Task<Result<CategoryResponse>> CreateCategoryAsync(int restaurantId, CreateCategoryRequest request, CancellationToken cancellationToken = default)
        {
            var existsRestaurant = await _dbContext.Restaurants.AnyAsync(r => r.Id == restaurantId, cancellationToken);
            if (!existsRestaurant)
                return Result.Failure<CategoryResponse>(RestaurantErrors.NotFound);

            var isDuplicate = await _dbContext.Categories
                .AnyAsync(c => c.RestaurantId == restaurantId && c.Name == request.Name, cancellationToken);
            if (isDuplicate)
                return Result.Failure<CategoryResponse>(CategoryErrors.AlreadyExists);

            var newCategory = request.Adapt<Category>();
            newCategory.RestaurantId = restaurantId;

            await _dbContext.Categories.AddAsync(newCategory, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await InvalidateCategoryCache(restaurantId, newCategory.Id);


            return Result.Success(newCategory.Adapt<CategoryResponse>());
        }

        public async Task<Result> UpdateCategoryAsync(int restaurantId, int categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
        {
            var existsRestaurant = await _dbContext.Restaurants.AnyAsync(r => r.Id == restaurantId, cancellationToken);
            if (!existsRestaurant)
                return Result.Failure<CategoryResponse>(RestaurantErrors.NotFound);

            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.RestaurantId == restaurantId, cancellationToken);
            if (existingCategory == null)
                return Result.Failure(CategoryErrors.NotFound);

            var isDuplicate = await _dbContext.Categories
                    .AnyAsync(c =>
                        c.Id != categoryId &&
                        c.RestaurantId == restaurantId &&
                        c.Name == request.Name,
                        cancellationToken
                    );

            if (isDuplicate)
                return Result.Failure(CategoryErrors.AlreadyExists);

            existingCategory.Name = request.Name;
            await _dbContext.SaveChangesAsync(cancellationToken);

            await InvalidateCategoryCache(restaurantId, categoryId);

            return Result.Success();

        }

        public async Task<Result> DeleteCategoryAsync(int restaurantId, int categoryId, CancellationToken cancellationToken = default)
        {
            var existsRestaurant = await _dbContext.Restaurants.AnyAsync(r => r.Id == restaurantId, cancellationToken);
            if (!existsRestaurant)
                return Result.Failure<CategoryResponse>(RestaurantErrors.NotFound);

            var existCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.RestaurantId == restaurantId, cancellationToken);
            if (existCategory is null)
                return Result.Failure(CategoryErrors.NotFound);

            _dbContext.Categories.Remove(existCategory);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await InvalidateCategoryCache(restaurantId, categoryId);

            return Result.Success();
        }

        public async Task<Result> ToggleAvilableStatusAsync(int restaurantId, int categoryId, CancellationToken cancellationToken = default)
        {
            var existsRestaurant = await _dbContext.Restaurants.AnyAsync(r => r.Id == restaurantId, cancellationToken);
            if (!existsRestaurant)
                return Result.Failure(RestaurantErrors.NotFound);

            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.RestaurantId == restaurantId && c.Id == categoryId, cancellationToken);
            if (existingCategory is null)
                return Result.Failure(CategoryErrors.NotFound);

            existingCategory.IsAvailable = !existingCategory.IsAvailable;
            await _dbContext.SaveChangesAsync(cancellationToken);

            await InvalidateCategoryCache(restaurantId, categoryId);

            return Result.Success();
        }

        private async Task InvalidateCategoryCache(int restaurantId, int? categoryId = null)
        {
            await _hybridCache.RemoveAsync($"{_CacheKeyPrefix}_{restaurantId}_ALL");
            await _hybridCache.RemoveAsync($"{_CacheKeyPrefix}_{restaurantId}_Available");

            if (categoryId.HasValue)
                await _hybridCache.RemoveAsync($"{_CacheKeyPrefix}_{restaurantId}_{categoryId.Value}");
        }
    }
}
