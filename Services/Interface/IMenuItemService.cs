using FoodFlow.Contracts.Categories;

namespace FoodFlow.Services.Interface
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemResponse>> GetAllItemsAsync(int restaurantId, int categoryId, CancellationToken cancellationToken = default);
        Task<MenuItemResponse?> GetItemByIdAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default);
        Task<MenuItemResponse?> CreateItemAsync(int restaurantId, int categoryId, CreateMenuItemRequest request, CancellationToken cancellationToken = default);
        Task<bool> UpdateItemAsync(int restaurantId, int categoryId, int itemId, UpdateMenuItemRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteItemAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default);
        Task<bool> ToggleAvaliableItemAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default);
    }
}
