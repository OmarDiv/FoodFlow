using Microsoft.Extensions.Caching.Hybrid;

namespace FoodFlow.Services.Impelement
{
    public class MenuItemService(ApplicationDbContext applicationDbContext, HybridCache hybridCache) : IMenuItemService
    {
        private readonly ApplicationDbContext _context = applicationDbContext;
        private readonly HybridCache _hybridCache = hybridCache;
        private const string _CacheKeyPrefix = "items";


        public async Task<Result<IEnumerable<MenuItemResponse>>> GetAllItemsAsync(int restaurantId, int categoryId, CancellationToken cancellationToken = default)
        {
            var exists = await _context.Categories.AnyAsync(c => c.Id == categoryId && c.RestaurantId == restaurantId);
            if (!exists)
                return Result.Failure<IEnumerable<MenuItemResponse>>(CategoryErrors.RestaruantOrCategoryNotFound);
            var cacheKey = $"{_CacheKeyPrefix}_{restaurantId}_{categoryId}_ALL";
            var items = await _hybridCache.GetOrCreateAsync<IEnumerable<MenuItemResponse>>
                (cacheKey, async entry =>
                    await _context.MenuItems
                      .Where(i => i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId)
                      .ProjectToType<MenuItemResponse>()
                      .AsNoTracking()
                      .ToListAsync(cancellationToken)
                );

            return Result.Success(items);
        }
        public async Task<Result<IEnumerable<MenuItemResponse>>> GetAvailableItemsAsync(int restaurantId, int categoryId, CancellationToken cancellationToken = default)
        {
            var exists = await _context.Categories.AnyAsync(c => c.Id == categoryId && c.RestaurantId == restaurantId);
            if (!exists)
                return Result.Failure<IEnumerable<MenuItemResponse>>(CategoryErrors.RestaruantOrCategoryNotFound);
            var cacheKey = $"{_CacheKeyPrefix}_{restaurantId}_{categoryId}_Available";
            var items = await _hybridCache.GetOrCreateAsync<IEnumerable<MenuItemResponse>>
                (cacheKey, async entry =>
                    await _context.MenuItems
                      .Where(i => i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId && i.IsAvailable == true)
                      .ProjectToType<MenuItemResponse>()
                      .AsNoTracking()
                      .ToListAsync(cancellationToken)
                );

            return Result.Success(items);
        }

        public async Task<Result<MenuItemResponse>> GetItemByIdAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{_CacheKeyPrefix}_{restaurantId}_{categoryId}_{itemId}";

            var item = await _hybridCache.GetOrCreateAsync(
                cacheKey,
                async entry =>
                   await _context.MenuItems
                .Where(i => i.Id == itemId && i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId)
                .ProjectToType<MenuItemResponse>()
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken)
                );

            return item is null ?
              Result.Failure<MenuItemResponse>(MenuItemErrors.NotFound)
            : Result.Success(item);
        }

        public async Task<Result<MenuItemResponse>> CreateItemAsync(int restaurantId, int categoryId, CreateMenuItemRequest request, CancellationToken cancellationToken = default)
        {
            var exists = await _context.Categories.AnyAsync(c => c.Id == categoryId && c.RestaurantId == restaurantId, cancellationToken);
            if (!exists)
                return Result.Failure<MenuItemResponse>(CategoryErrors.RestaruantOrCategoryNotFound);

            var itemExists = await _context.MenuItems
               .AnyAsync(i => i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId && i.Name == request.Name, cancellationToken);

            if (itemExists)
                return Result.Failure<MenuItemResponse>(MenuItemErrors.AlreadyExists);

            var newItem = request.Adapt<MenuItem>();
            newItem.CategoryId = categoryId;
            try
            {
                await _context.MenuItems.AddAsync(newItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await InvalidateCategoryCache(restaurantId, categoryId, newItem.Id);
            }
            catch
            {
                return Result.Failure<MenuItemResponse>(MenuItemErrors.FailedToCreate);
            }

            var response = newItem.Adapt<MenuItemResponse>();
            return Result.Success(response);
        }

        public async Task<Result> UpdateItemAsync(int restaurantId, int categoryId, int itemId, UpdateMenuItemRequest request, CancellationToken cancellationToken = default)
        {
            var existingItem = await _context.MenuItems
                .Where(i => i.Id == itemId && i.CategoryId == categoryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingItem is null)
                return Result.Failure(MenuItemErrors.NotFound);

            try
            {
                request.Adapt(existingItem);
                await _context.SaveChangesAsync(cancellationToken);
                await InvalidateCategoryCache(restaurantId, categoryId, itemId);
            }
            catch
            {
                return Result.Failure(MenuItemErrors.FailedToUpdate);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteItemAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default)
        {
            var existingItem = await _context.MenuItems
                .Where(i => i.Id == itemId && i.CategoryId == categoryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingItem is null)
                return Result.Failure(MenuItemErrors.NotFound);

            try
            {
                _context.Remove(existingItem);
                await _context.SaveChangesAsync(cancellationToken);
                await InvalidateCategoryCache(restaurantId, categoryId, itemId);
            }
            catch
            {
                return Result.Failure(MenuItemErrors.FailedToDelete);
            }

            return Result.Success();
        }

        public async Task<Result> ToggleAvaliableItemAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default)
        {
            var existingItem = await _context.MenuItems
                .Where(i => i.Id == itemId && i.CategoryId == categoryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingItem is null)
                return Result.Failure(MenuItemErrors.NotFound);

            try
            {
                existingItem.IsAvailable = !existingItem.IsAvailable;
                var saveResult = await _context.SaveChangesAsync(cancellationToken);
                await InvalidateCategoryCache(restaurantId, categoryId, itemId);

            }
            catch
            {
                return Result.Failure(MenuItemErrors.FailedToToggleAvailability);
            }

            return Result.Success();
        }

        private async Task InvalidateCategoryCache(int restaurantId, int categoryId, int? itemId = null)
        {
            await _hybridCache.RemoveAsync($"{_CacheKeyPrefix}_{restaurantId}_{categoryId}_ALL");
            await _hybridCache.RemoveAsync($"{_CacheKeyPrefix}_{restaurantId}_{categoryId}_Available");
            if (itemId.HasValue)
                await _hybridCache.RemoveAsync($"{_CacheKeyPrefix}_{restaurantId}_{categoryId}_{itemId.Value}");
        }
    }
}