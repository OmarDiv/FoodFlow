using FoodFlow.Contracts.Categories;

namespace FoodFlow.Services.Impelement
{
    public class CategoryService(ApplicationDbContext applicationDbContext) : ICategoryService
    {
        private readonly ApplicationDbContext _context = applicationDbContext;

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(int restaurantId, CancellationToken cancellationToken = default)
        {
            var categories = await _context.Categories.Where(c => c.Restaurant.Id == restaurantId)
                .Include(c => c.MenuItems)
                .Include(c => c.Restaurant)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (categories is null)
                return null!;


            return categories.Adapt<IEnumerable<CategoryResponse>>();
        }

        public async Task<CategoryResponse?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await _context.Categories.Where(c => c.Id == id).Include(r => r.Restaurant).Include(m => m.MenuItems).FirstOrDefaultAsync(cancellationToken);
            if (category is null)
                return null!;
            return category.Adapt<CategoryResponse>();
        }

        public async Task<CategoryResponse?> CreateCategoryAsync(int restaurantId,CreateCategoryRequest request,CancellationToken cancellationToken = default)
        {
            var exists = await _context.Restaurants
                .AnyAsync(r => r.Id == restaurantId, cancellationToken);
            if (!exists)
                return null; 

            var category = new Category
            {
                Name = request.Name,
                RestaurantId = restaurantId
            };

            await _context.Categories.AddAsync(category, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return category.Adapt<CategoryResponse>();
        }


        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
        {
            // 1. التأكد من وجود الكاتيجوري
            var existingCategory = await _context.Categories.FindAsync( id , cancellationToken);
            if (existingCategory == null)
                return false;

            var isDuplicate = await _context.Categories
                .AnyAsync(c =>
                    c.Id != id &&
                    c.RestaurantId == existingCategory.RestaurantId &&
                    c.Name.ToLower() == request.Name.ToLower(),
                    cancellationToken);

            if (isDuplicate)
                return false; // ممكن تعيده بـ Conflict لاحقًا

            // 3. التعديل
            existingCategory.Name = request.Name;

            // 4. الحفظ
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
        {
            var existCategory = await _context.Categories.FindAsync(id, cancellationToken);
            if (existCategory is null)
                return false; 
            _context.Categories.Remove(existCategory);
            await _context.SaveChangesAsync(cancellationToken);
            return true;

        }
    }
}
