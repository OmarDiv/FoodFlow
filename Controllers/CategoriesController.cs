using FoodFlow.Contracts.Categories;
namespace FoodFlow.Controllers
{
    [Route("api/restaurants/{restaurantId}/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        [HttpGet()]
        public async Task<IActionResult> GetAll([FromRoute] int restaurantId, CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(restaurantId, cancellationToken);
            if (categories is null)
                return NotFound("No categories found for the specified restaurant.");
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetById([FromRoute] int restaurantId, [FromRoute] int categoryId, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(restaurantId, categoryId, cancellationToken);
            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] int restaurantId, [FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.CreateCategoryAsync(restaurantId, request, cancellationToken);
            if (category is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById), new { restaurantId, categoryId = category.Id }, category);
        }


        [HttpPut("{categoryId}")]
        //[Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> Update(int restaurantId, int categoryId, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var restaurant = await _categoryService.UpdateCategoryAsync(restaurantId, categoryId, request, cancellationToken);
            if (!restaurant)
                return NotFound("Category no exsist");


            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        //[Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> Delete([FromRoute] int restaurantId, [FromRoute] int categoryId, CancellationToken cancellationToken)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(restaurantId, categoryId, cancellationToken);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}