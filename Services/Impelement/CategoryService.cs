using FoodFlow.Contracts.Categories;

namespace FoodFlow.Services.Impelement
{
    public class CategoryService(ApplicationDbContext applicationDbContext) : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext = applicationDbContext;

        public async Task<Result<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync(int restaurantId, CancellationToken cancellationToken = default)
        {
            var categories = await _dbContext.Categories
                .Where(c => c.RestaurantId == restaurantId)
                .ProjectToType<CategoryResponse>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<Result<CategoryResponse>> GetCategoryByIdAsync(int restaurantId, int id, CancellationToken cancellationToken = default)
        {
            var category = await _dbContext.Categories
                                 .Where(c => c.Id == id && c.RestaurantId == restaurantId)
                                 .ProjectToType<CategoryResponse>()
                                 .FirstOrDefaultAsync(cancellationToken);
            return category is null
                 ? Result.Failure<CategoryResponse>(CategoryErrors.NotFound)
                 : Result.Success(category);
        }

        public async Task<Result<CategoryResponse>> CreateCategoryAsync(int restaurantId, CreateCategoryRequest request, CancellationToken cancellationToken = default)
        {
            var exists = await _dbContext.Restaurants.AnyAsync(r => r.Id == restaurantId, cancellationToken);
            if (!exists)
                return Result.Failure<CategoryResponse>(RestaurantErrors.NotFound);

            var isDuplicate = await _dbContext.Categories
                .AnyAsync(c => c.RestaurantId == restaurantId && c.Name.ToLower() == request.Name.ToLower(), cancellationToken);
            if (isDuplicate)
                return Result.Failure<CategoryResponse>(CategoryErrors.AlreadyExists);

            var newCategory = request.Adapt<Category>();
            newCategory.RestaurantId = restaurantId;

            await _dbContext.Categories.AddAsync(newCategory, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(newCategory.Adapt<CategoryResponse>());
        }

        public async Task<Result> UpdateCategoryAsync(int restaurantId, int categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
        {
            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.RestaurantId == restaurantId, cancellationToken);
            if (existingCategory == null)
                return Result.Failure(CategoryErrors.NotFound);

            var isDuplicate = await _dbContext.Categories
                    .AnyAsync(c =>
                        c.Id != categoryId &&
                        c.RestaurantId == restaurantId &&
                        c.Name.ToLower() == request.Name.ToLower(),
                        cancellationToken);

            if (isDuplicate)
                return Result.Failure(CategoryErrors.AlreadyExists);

            existingCategory.Name = request.Name;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }

        public async Task<Result> DeleteCategoryAsync(int restaurantId, int categoryId, CancellationToken cancellationToken = default)
        {
            var existCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.RestaurantId == restaurantId, cancellationToken);
            if (existCategory is null)
                return Result.Failure(CategoryErrors.NotFound);

            _dbContext.Categories.Remove(existCategory);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> ToggleActiveStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(id, cancellationToken);

            if (existingCategory is null)
                return Result.Failure(CategoryErrors.NotFound);

            existingCategory.IsAvailble = !existingCategory.IsAvailble;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
