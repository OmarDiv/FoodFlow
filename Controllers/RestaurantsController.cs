
namespace FoodFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController(IRestaurantService restaurantService) : ControllerBase
    {
        public readonly IRestaurantService _restaurantService = restaurantService;

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _restaurantService.GetAllRestaurantsAsync(cancellationToken);

            return result.IsSuccess
                 ? Ok(result.Value)
                : result.ToProblem();
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
        {
            var result = await _restaurantService.GetAllRestaurantsAsync(cancellationToken);

            return result.IsSuccess
                 ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.GetRestaurantByIdAsync(id);
            return result.IsSuccess
                 ? Ok(result.Value)
                : result.ToProblem();
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantRequest request, CancellationToken cancellationToken)
        {

            var result = await _restaurantService.CreateRestaurantAsync(request, cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.UpdateRestaurantAsync(id, request, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.DeleteRestaurantAsync(id, cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
        [HttpPut("{id}/toggle-open")]
        public async Task<IActionResult> ToggleOpenStatus(int id, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.ToggleOpenStatusAsync(id, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
        [HttpPut("{id}/toggle-active")]
        public async Task<IActionResult> ToggleActiveStatus(int id, CancellationToken cancellationToken = default)
        {
            var result = await _restaurantService.ToggleActiveStatusAsync(id, cancellationToken);
            return result.IsSuccess
               ? NoContent()
               : result.ToProblem();
        }

    }
}