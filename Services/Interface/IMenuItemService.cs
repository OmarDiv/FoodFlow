using FoodFlow.Contracts.Categories;

namespace FoodFlow.Services.Interface
{
    public interface IMenuItemService
    {
        Task<List<MenuItemResponse>> GetAllMenuItemsAsync(int restaurantId, CancellationToken cancellationToken = default); //عرض كل الأصناف لمطعم.
        Task<MenuItemResponse?> GetMenuItemByIdAsync(int id, CancellationToken cancellationToken = default);// عرض صنف معين من الأصناف.
        Task<MenuItemResponse> CreateMenuItemAsync(CreateMenuItemRequest request, CancellationToken cancellationToken = default); //إضافة صنف جديد.
        Task<bool> UpdateMenuItemAsync(int id, UpdateMenuItemRequest request, CancellationToken cancellationToken = default); //تحديث صنف موجود.
        Task<bool> DeleteMenuItemAsync(int id, CancellationToken cancellationToken = default); //حذف صنف موجود.
        Task<bool> ToggleAvailabilityAsync(int id, CancellationToken cancellationToken = default); //تغيير حالة توفر الصنف (متاح/غير متاح).
    }
}
