
namespace FoodFlow.Services.Impelement
{
    public class MenuItemService : IMenuItemService
    {
        public Task<MenuItemResponse> CreateMenuItemAsync(CreateMenuItemRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMenuItemAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<MenuItemResponse>> GetAllMenuItemsAsync(int restaurantId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MenuItemResponse?> GetMenuItemByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ToggleAvailabilityAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMenuItemAsync(int id, UpdateMenuItemRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
