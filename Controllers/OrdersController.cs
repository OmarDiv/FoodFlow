using Microsoft.AspNetCore.Authorization;

namespace FoodFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _orderService.CreateOrderAsync(request, userId, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _orderService.GetOrderByIdAsync(orderId, userId, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpGet("customer")]
        public async Task<IActionResult> GetOrdersForCustomer(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _orderService.GetOrdersForCustomerAsync(userId!, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpPost("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _orderService.CancelOrderAsync(orderId, userId, cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetOrdersForRestaurant(int restaurantId, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetOrdersForRestaurantAsync(restaurantId, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpGet("restaurant/{restaurantId}/pending")]
        public async Task<IActionResult> GetPendingOrdersForRestaurant(int restaurantId, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetPendingOrdersForRestaurantAsync(restaurantId, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpPost("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatus newStatus, CancellationToken cancellationToken)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, newStatus, cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
    }
}