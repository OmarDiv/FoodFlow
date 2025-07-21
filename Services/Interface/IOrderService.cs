namespace FoodFlow.Services.Interface
{
    public interface IOrderService
    {
        Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderRequest request, string userId, CancellationToken cancellationToken = default);
        Task<Result<OrderResponse>> GetOrderByIdAsync(int orderId, string userId, CancellationToken cancellationToken = default);
        Task<Result<List<OrderResponse>>> GetOrdersForCustomerAsync(string userId, CancellationToken cancellationToken = default);
        Task<Result> CancelOrderAsync(int orderId, string userId, CancellationToken cancellationToken = default);
        Task<Result<List<OrderResponse>>> GetOrdersForRestaurantAsync(int restaurantId, CancellationToken cancellationToken = default);
        Task<Result<List<OrderResponse>>> GetPendingOrdersForRestaurantAsync(int restaurantId, CancellationToken cancellationToken = default);
        Task<Result> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus, CancellationToken cancellationToken = default);
    }
}

