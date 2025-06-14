namespace FoodFlow.Services.Impelement
{
    public class MenuItemService(ApplicationDbContext applicationDbContext) : IMenuItemService
    {
        private readonly ApplicationDbContext _context = applicationDbContext;

        public async Task<IEnumerable<MenuItemResponse>> GetAllItemsAsync(int restaurantId, int categoryId, CancellationToken cancellationToken = default)
        {
            var items = await _context.MenuItems
                .Where(i => i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId && i.IsAvailable == true)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            return items.Adapt<IEnumerable<MenuItemResponse>>();
        }

        public async Task<MenuItemResponse?> GetItemByIdAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default)
        {
            var item = await _context.MenuItems
               .Where(i => i.Id == itemId && i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId && i.IsAvailable == true)
               .AsNoTracking()
               .FirstOrDefaultAsync(cancellationToken);
            if (item is null)
                return null;
            return item?.Adapt<MenuItemResponse>();
        }
        public async Task<MenuItemResponse?> CreateItemAsync(int restaurantId, int categoryId, CreateMenuItemRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
                return null;

            var ItemExists = await _context.MenuItems
               .AnyAsync(i => i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId && i.Name.ToLower() == request.Name.ToLower(), cancellationToken);

            if (ItemExists)
                return null;
            var newItem = request.Adapt<MenuItem>();
            newItem.CategoryId = categoryId;

            await _context.MenuItems.AddAsync(newItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return newItem.Adapt<MenuItemResponse>();
        }

        public async Task<bool> UpdateItemAsync(int restaurantId, int categoryId, int itemId, UpdateMenuItemRequest request, CancellationToken cancellationToken = default)
        {
            var existingItem = await _context.MenuItems.Where(i => i.Id == itemId && i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId).FirstOrDefaultAsync(cancellationToken);
            if (existingItem is null)
                return false;
            request.Adapt(existingItem);
            _context.Update(existingItem);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteItemAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default)
        {
            var existingItem = await _context.MenuItems.Where(i => i.Id == itemId && i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId).FirstOrDefaultAsync(cancellationToken);
            if (existingItem is null)
                return false;

            _context.Remove(existingItem);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> ToggleAvaliableItemAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default)
        {
            var existingItem = await _context.MenuItems.Where(i => i.Id == itemId && i.CategoryId == categoryId && i.Category.RestaurantId == restaurantId).FirstOrDefaultAsync(cancellationToken);
            if (existingItem is null)
                return false;
            existingItem.IsAvailable = !existingItem.IsAvailable;
           await _context.SaveChangesAsync();
            return true;

        }
    }
}
