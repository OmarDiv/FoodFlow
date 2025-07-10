namespace FoodFlow.Services.Impelement
{
    public class MenuItemService(ApplicationDbContext applicationDbContext) : IMenuItemService
    {
        private readonly ApplicationDbContext _context = applicationDbContext;

        public async Task<Result<IEnumerable<MenuItemResponse>>> GetAllItemsAsync(int restaurantId, int categoryId, CancellationToken cancellationToken = default)
        {
            var items = await _context.MenuItems
                .Where(i => i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId && i.IsAvailable == true)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!items.Any())
                return Result.Failure<IEnumerable<MenuItemResponse>>(MenuItemErrors.NoItemsFound);

            var response = items.Adapt<IEnumerable<MenuItemResponse>>();
            return Result.Success(response);
        }

        public async Task<Result<MenuItemResponse>> GetItemByIdAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default)
        {
            var item = await _context.MenuItems
               .Where(i => i.Id == itemId && i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId && i.IsAvailable == true)
               .AsNoTracking()
               .FirstOrDefaultAsync(cancellationToken);

            if (item is null)
                return Result.Failure<MenuItemResponse>(MenuItemErrors.NotFound);

            var response = item.Adapt<MenuItemResponse>();
            return Result.Success(response);
        }

        public async Task<Result<MenuItemResponse>> CreateItemAsync(int restaurantId, int categoryId, CreateMenuItemRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
                return Result.Failure<MenuItemResponse>(MenuItemErrors.InvalidRequest);

            var itemExists = await _context.MenuItems
               .AnyAsync(i => i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId && i.Name.ToLower() == request.Name.ToLower(), cancellationToken);

            if (itemExists)
                return Result.Failure<MenuItemResponse>(MenuItemErrors.AlreadyExists);

            var newItem = request.Adapt<MenuItem>();
            newItem.CategoryId = categoryId;

            try
            {
                await _context.MenuItems.AddAsync(newItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
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
                .Where(i => i.Id == itemId && i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingItem is null)
                return Result.Failure(MenuItemErrors.NotFound);

            request.Adapt(existingItem);

            try
            {
                _context.Update(existingItem);
                var saveResult = await _context.SaveChangesAsync(cancellationToken);
                if (saveResult == 0)
                    return Result.Failure(MenuItemErrors.FailedToUpdate);
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
                .Where(i => i.Id == itemId && i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingItem is null)
                return Result.Failure(MenuItemErrors.NotFound);

            try
            {
                _context.Remove(existingItem);
                var saveResult = await _context.SaveChangesAsync(cancellationToken);
                if (saveResult == 0)
                    return Result.Failure(MenuItemErrors.FailedToDelete);
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
                .Where(i => i.Id == itemId && i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingItem is null)
                return Result.Failure(MenuItemErrors.NotFound);

            try
            {
                existingItem.IsAvailable = !existingItem.IsAvailable;
                var saveResult = await _context.SaveChangesAsync(cancellationToken);
                if (saveResult == 0)
                    return Result.Failure(MenuItemErrors.FailedToToggleAvailability);
            }
            catch
            {
                return Result.Failure(MenuItemErrors.FailedToToggleAvailability);
            }

            return Result.Success();
        }
    }
}