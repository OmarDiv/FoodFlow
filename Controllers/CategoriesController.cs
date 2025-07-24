using FoodFlow.Contracts.Categories;
using Microsoft.AspNetCore.Authorization;

namespace FoodFlow.Controllers
{
    [ApiController]
    [Route("api/restaurants/{restaurantId}/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private const string _CacheKeyPrefix = "categories";
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] int restaurantId, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetAllCategoriesAsync(restaurantId, cancellationToken);
            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetById([FromRoute] int restaurantId, [FromRoute] int categoryId, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetCategoryByIdAsync(restaurantId, categoryId, cancellationToken);
            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] int restaurantId, [FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoryService.CreateCategoryAsync(restaurantId, request, cancellationToken);
            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById), new { restaurantId, categoryId = result.Value!.Id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("{categoryId}")]
        //[Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> Update([FromRoute] int restaurantId, [FromRoute] int categoryId, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoryService.UpdateCategoryAsync(restaurantId, categoryId, request, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

        [HttpDelete("{categoryId}")]
        //[Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> Delete([FromRoute] int restaurantId, [FromRoute] int categoryId, CancellationToken cancellationToken)
        {
            var result = await _categoryService.DeleteCategoryAsync(restaurantId, categoryId, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
        [HttpPut("{categoryId}/toggle-status")]
        //[Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int restaurantId, [FromRoute] int categoryId, CancellationToken cancellationToken)
        {
            var result = await _categoryService.ToggleAvilableStatusAsync(restaurantId, categoryId, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
    }
}