using Microsoft.AspNetCore.Authorization;
namespace FoodFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantsController(IRestaurantService restaurantService) : ControllerBase
    {
        private readonly IRestaurantService _restaurantService = restaurantService;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _restaurantService.GetAllRestaurantsAsync(cancellationToken);
            return Ok(result.Value);
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
        {
            var result = await _restaurantService.GetActiveRestaurantsAsync(cancellationToken);

            return Ok(result.Value);
        }
        [HttpGet("{restaurantId}")]
        public async Task<IActionResult> GetById([FromRoute] int restaurantId, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.GetRestaurantByIdAsync(restaurantId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantRequest request, CancellationToken cancellationToken)
        {
            var result = await _restaurantService.CreateRestaurantAsync(request, cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById), new { restaurantId = result.Value!.Id }, result.Value)
                : result.ToProblem();
        }
        [HttpPut("{restaurantId}")]
        public async Task<IActionResult> Update([FromRoute] int restaurantId, [FromBody] UpdateRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.UpdateRestaurantAsync(restaurantId, request, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
        [HttpDelete("{restaurantId}")]
        public async Task<IActionResult> Delete([FromRoute] int restaurantId, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.DeleteRestaurantAsync(restaurantId, cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
        [HttpPut("{restaurantId}/toggle-open")]
        public async Task<IActionResult> ToggleOpenStatus([FromRoute] int restaurantId, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.ToggleOpenStatusAsync(restaurantId, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
        [HttpPut("{restaurantId}/toggle-active")]
        public async Task<IActionResult> ToggleActiveStatus([FromRoute] int restaurantId, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.ToggleActiveStatusAsync(restaurantId, cancellationToken);
            return result.IsSuccess
               ? NoContent()
               : result.ToProblem();
        }
    }
}