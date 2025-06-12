using FoodFlow.Contracts.Categories;

namespace FoodFlow.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(int restaurantId, CancellationToken cancellationToken = default);
        Task<CategoryResponse?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<CategoryResponse?> CreateCategoryAsync(int id,CreateCategoryRequest request, CancellationToken cancellationToken = default);
        Task<bool> UpdateCategoryAsync(int id, UpdateCategoryRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default);
    }
}
