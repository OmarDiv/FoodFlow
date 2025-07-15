using FoodFlow.Contracts.Categories;

namespace FoodFlow.Services.Interface
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync(int restaurantId, CancellationToken cancellationToken = default);
        Task<Result<CategoryResponse>> GetCategoryByIdAsync(int restaurantId, int categoryId, CancellationToken cancellationToken = default);
        Task<Result<CategoryResponse>> CreateCategoryAsync(int restaurantId, CreateCategoryRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateCategoryAsync(int restaurantId, int categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteCategoryAsync(int restaurantId, int categoryId, CancellationToken cancellationToken = default);
        Task<Result> ToggleActiveStatusAsync(int id, CancellationToken cancellationToken = default);
    }
}
