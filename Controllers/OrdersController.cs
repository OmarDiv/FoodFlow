//using FoodFlow.Contracts.Orders;
//using System.Security.Claims;

//[ApiController]
//[Route("api/[controller]")]
//public class OrdersController(IOrderService orderService) : ControllerBase
//{
//    private readonly IOrderService _orderService = orderService;

//    [HttpPost]
//    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
//    {
//        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//        var order = await _orderService.CreateOrderAsync(request, userId!);
//        return Ok(order);
//    }

//    [HttpGet]
//    public async Task<IActionResult> GetMyOrders()
//    {
//        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//        var orders = await _orderService.GetOrdersForCustomerAsync(userId!);
//        return Ok(orders);
//    }

//    [HttpGet("{id}")]
//    public async Task<IActionResult> GetOrder(int id)
//    {
//        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//        var order = await _orderService.GetOrderByIdAsync(id, userId!);
//        return Ok(order);
//    }

//    [HttpPost("{id}/cancel")]
//    public async Task<IActionResult> CancelOrder(int id)
//    {
//        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//        await _orderService.CancelOrderAsync(id, userId!);
//        return NoContent();
//    }

//    // ========== للمطعم ==========

//    [HttpGet("restaurant/{restaurantId}")]
//    //[Authorize(Roles = "RestaurantOwner")]
//    public async Task<IActionResult> GetRestaurantOrders(int restaurantId)
//    {
//        var orders = await _orderService.GetOrdersForRestaurantAsync(restaurantId);
//        return Ok(orders);
//    }

//    [HttpGet("restaurant/{restaurantId}/pending")]
//    [Authorize(Roles = "RestaurantOwner")]
//    public async Task<IActionResult> GetPendingOrders(int restaurantId)
//    {
//        var orders = await _orderService.GetPendingOrdersForRestaurantAsync(restaurantId);
//        return Ok(orders);
//    }

//    [HttpPut("{id}/status")]
//    [Authorize(Roles = "RestaurantOwner")]
//    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request)
//    {
//        var updated = await _orderService.UpdateOrderStatusAsync(id, request);
//        if (updated is not true)
//            return BadRequest(); // أو Forbid() حسب السيناريو
//        return NoContent();
//    }
//}