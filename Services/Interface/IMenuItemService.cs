using FoodFlow.Contracts.Categories;

namespace FoodFlow.Services.Interface
{
    public interface IMenuItemService
    {
        Task<Result<IEnumerable<MenuItemResponse>>> GetAllItemsAsync(int restaurantId, int categoryId, CancellationToken cancellationToken = default);
        Task<Result<MenuItemResponse>> GetItemByIdAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default);
        Task<Result<MenuItemResponse>> CreateItemAsync(int restaurantId, int categoryId, CreateMenuItemRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateItemAsync(int restaurantId, int categoryId, int itemId, UpdateMenuItemRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteItemAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default);
        Task<Result> ToggleAvaliableItemAsync(int restaurantId, int categoryId, int itemId, CancellationToken cancellationToken = default);
    }
}
