namespace FoodFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoController(IGeoapifyService geoapifyService) : ControllerBase
    {
        private readonly IGeoapifyService _geoapifyService = geoapifyService;

        [HttpGet("reverse")]
        public async Task<IActionResult> GetAddress(double lat, double lon)
        {
            var result = await _geoapifyService.GetAddressFromCoordinatesAsync(lat, lon);

            if (result is null)
                return NotFound("Address not found.");

            return Ok(result);
        }
    }
}
