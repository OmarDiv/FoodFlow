using FoodFlow.Contracts.Categories;

namespace FoodFlow.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(int restaurantId, CancellationToken cancellationToken = default);
        Task<CategoryResponse?> GetCategoryByIdAsync(int restaurantId,int categoryId, CancellationToken cancellationToken = default);
        Task<CategoryResponse?> CreateCategoryAsync(int restaurantId, CreateCategoryRequest request, CancellationToken cancellationToken = default);
        Task<bool> UpdateCategoryAsync(int restaurantId,int categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteCategoryAsync(int restaurantId,int categoryId, CancellationToken cancellationToken = default);
    }
}
