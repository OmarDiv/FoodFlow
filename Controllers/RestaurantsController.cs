using FoodFlow.Abstractions.Consts;
using FoodFlow.Abstractions.Filters;
using Microsoft.AspNetCore.Authorization;
namespace FoodFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController(IRestaurantService restaurantService) : ControllerBase
    {
        private readonly IRestaurantService _restaurantService = restaurantService;

        [HttpGet]
        [HasPermission(Permissions.ViewRestaurants)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _restaurantService.GetAllRestaurantsAsync(cancellationToken);
            return Ok(result.Value);
        }
        [HttpGet("active")]
        [HasPermission(Permissions.ViewRestaurants)]
        public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
        {
            var result = await _restaurantService.GetActiveRestaurantsAsync(cancellationToken);

            return Ok(result.Value);
        }
        [HttpGet("{restaurantId}")]
        [HasPermission(Permissions.ViewRestaurants)]
        public async Task<IActionResult> GetById([FromRoute] int restaurantId, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.GetRestaurantByIdAsync(restaurantId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost]
        [HasPermission(Permissions.CreateRestaurant)]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantRequest request, CancellationToken cancellationToken)
        {
            var result = await _restaurantService.CreateRestaurantAsync(request, cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById), new { restaurantId = result.Value!.Id }, result.Value)
                : result.ToProblem();
        }
        [HttpPut("{restaurantId}")]
        [HasPermission(Permissions.UpdateRestaurant)]
        public async Task<IActionResult> Update([FromRoute] int restaurantId, [FromBody] UpdateRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.UpdateRestaurantAsync(restaurantId, request, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
        [HttpDelete("{restaurantId}")]
        [HasPermission(Permissions.DeleteRestaurant)]
        public async Task<IActionResult> Delete([FromRoute] int restaurantId, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.DeleteRestaurantAsync(restaurantId, cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
        [HttpPut("{restaurantId}/toggle-open")]
        [HasPermission(Permissions.ToggleRestaurantOpen)]
        public async Task<IActionResult> ToggleOpenStatus([FromRoute] int restaurantId, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.ToggleOpenStatusAsync(restaurantId, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
        [HttpPut("{restaurantId}/toggle-active")]
        [HasPermission(Permissions.ToggleRestaurantActive)]
        public async Task<IActionResult> ToggleActiveStatus([FromRoute] int restaurantId, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.ToggleActiveStatusAsync(restaurantId, cancellationToken);
            return result.IsSuccess
               ? NoContent()
               : result.ToProblem();
        }
    }
}