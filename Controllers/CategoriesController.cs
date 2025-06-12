using FoodFlow.Contracts.Categories;
namespace FoodFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        [HttpGet("GetAll/{id}")]
        public async Task<IActionResult> GetAll([FromRoute] int id, CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(id, cancellationToken);
            if (categories is null)
                return NotFound("No categories found for the specified restaurant.");
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);
            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Create( int id,[FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.CreateCategoryAsync(id,request, cancellationToken);
            if (category is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }


        [HttpPut("{id}")]
        //[Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var restaurant = await _categoryService.UpdateCategoryAsync(id, request, cancellationToken);
            if (!restaurant)
                return NotFound("Category no exsist");


            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id, cancellationToken);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}