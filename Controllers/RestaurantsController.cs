using FoodFlow.Contracts.Restaurants.Dtos;
namespace FoodFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController(IRestaurantService restaurantService) : ControllerBase
    {
        public readonly IRestaurantService _restaurantService = restaurantService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
           
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var restaurant = await _restaurantService.GetRestaurntByIdAsync(id);

            if (restaurant is null)
                return NotFound();

            var result = restaurant.Adapt<RestaurantListResponse>();
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantRequest request)
        {
            
            var Data = await _restaurantService.CreateRestaurantAsync(request);
            if(Data is null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById),Data.Id,Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRestaurantRequest request) 
        {
            var restaurant = await _restaurantService.UpdateRestaurantAsync(id, request);
            if (!restaurant)
                return NotFound("Restaurant no exsist");


            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _restaurantService.DeleteRestaurantAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

    }
}