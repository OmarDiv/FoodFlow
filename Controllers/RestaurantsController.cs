using Microsoft.AspNetCore.Authorization;

namespace FoodFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController(IRestaurantService restaurantService) : ControllerBase
    {
        public readonly IRestaurantService _restaurantService = restaurantService;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();

            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);

            if (restaurant is null)
                return NotFound();

            var result = restaurant.Adapt<RestaurantListResponse>();
            return Ok(result);
        }


        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantRequest request, CancellationToken cancellationToken)
        {

            var item = await _restaurantService.CreateRestaurantAsync(request, cancellationToken);
            if (item is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            var restaurant = await _restaurantService.UpdateRestaurantAsync(id, request, cancellationToken);
            if (!restaurant)
                return NotFound("Restaurant no exsist");


            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _restaurantService.DeleteRestaurantAsync(id, cancellationToken);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        [HttpPut("{id}/toggle-open")]
        public async Task<IActionResult> ToggleOpenStatus(int id, CancellationToken cancellationToken = default)
        {
            var restaurant = await _restaurantService.ToggleOpenStatusAsync(id, cancellationToken);
            if (!restaurant)
                return NotFound("Restaurant no exsist");


            return NoContent();
        }
        [HttpPut("{id}/toggle-active")]
        public async Task<IActionResult> ToggleActiveStatus(int id, CancellationToken cancellationToken = default)
        {
            var restaurant = await _restaurantService.ToggleActiveStatusAsync(id, cancellationToken);
            if (!restaurant)
                return NotFound("Restaurant no exsist");


            return NoContent();
        }

    }
}