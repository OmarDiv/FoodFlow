using FoodFlow.Const.Enum;

namespace FoodFlow.Services.Impelement
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderRequest request, string userId, CancellationToken cancellationToken = default)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync(request.RestaurantId, cancellationToken);
            if (restaurant is null)
                return Result.Failure<OrderResponse>(OrderErrors.RestaurantNotFound);

            var menuItemIds = request.Items.Select(i => i.MenuItemId).ToList();

            // هات كل الـ MenuItems المطلوبة مع الـ Category بتاعها
            var menuItems = await _dbContext.MenuItems
                .Include(m => m.Category)
                .Where(m => menuItemIds.Contains(m.Id))
                .ToListAsync(cancellationToken);

            // تحقق إن كل الـ MenuItems فعلاً تابعة للمطعم عن طريق الـ Category
            var allBelongToRestaurant = menuItems.All(m => m.Category.RestaurantId == request.RestaurantId);

            if (menuItems.Count != menuItemIds.Count || !allBelongToRestaurant)
                return Result.Failure<OrderResponse>(OrderErrors.InvalidMenuItems);

            var order = new Order
            {
                DeliveryAddress = "Customer Address Placeholder", // استبدلها بالعنوان الفعلي
                CustomerId = userId,
                RestaurantId = request.RestaurantId,
                Status = OrderStatus.Pending,
                OrderItems = request.Items.Select(item =>
                {
                    var menuItem = menuItems.First(m => m.Id == item.MenuItemId);
                    return new OrderItem
                    {
                        MenuItemId = item.MenuItemId,
                        Quantity = item.Quantity,
                        UnitPrice = menuItem.Price
                    };
                }).ToList()
            };

            order.TotalAmount = order.OrderItems.Sum(x => x.UnitPrice * x.Quantity);

            await _dbContext.Orders.AddAsync(order, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = order.Adapt<OrderResponse>();
            return Result.Success(response);
        }

        public async Task<Result<OrderResponse>> GetOrderByIdAsync(int orderId, string userId, CancellationToken cancellationToken = default)
        {
            var order = await _dbContext.Orders
                .Where(o => o.Id == orderId && o.CustomerId == userId)
                .ProjectToType<OrderResponse>()
                .FirstOrDefaultAsync(cancellationToken);

            if (order is null)
                return Result.Failure<OrderResponse>(OrderErrors.NotFound);

            return Result.Success(order);
        }

        public async Task<Result<List<OrderResponse>>> GetOrdersForCustomerAsync(string userId, CancellationToken cancellationToken = default)
        {
            var orders = await _dbContext.Orders
                .Where(o => o.CustomerId == userId)
                .OrderByDescending(o => o.CreatedOn)
                .ProjectToType<OrderResponse>()
                .ToListAsync(cancellationToken);

            if (!orders.Any())
                return Result.Failure<List<OrderResponse>>(OrderErrors.NoOrdersFound);

            return Result.Success(orders);
        }

        public async Task<Result> CancelOrderAsync(int orderId, string userId, CancellationToken cancellationToken = default)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
            if (order is null)
                return Result.Failure(OrderErrors.NotFound);

            if (order.CustomerId != userId)
                return Result.Failure(OrderErrors.Unauthorized);

            if (order.Status != OrderStatus.Pending)
                return Result.Failure(OrderErrors.CannotCancel);

            order.Status = OrderStatus.Cancelled;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<List<OrderResponse>>> GetOrdersForRestaurantAsync(int restaurantId, CancellationToken cancellationToken = default)
        {
            var orders = await _dbContext.Orders
                .Where(o => o.RestaurantId == restaurantId)
                .OrderByDescending(o => o.CreatedOn)
                .ProjectToType<OrderResponse>()
                .ToListAsync(cancellationToken);

            if (!orders.Any())
                return Result.Failure<List<OrderResponse>>(OrderErrors.NoOrdersFound);

            return Result.Success(orders);
        }

        public async Task<Result<List<OrderResponse>>> GetPendingOrdersForRestaurantAsync(int restaurantId, CancellationToken cancellationToken = default)
        {
            var orders = await _dbContext.Orders
                .Where(o => o.RestaurantId == restaurantId && o.Status == OrderStatus.Pending)
                .OrderByDescending(o => o.CreatedOn)
                .ProjectToType<OrderResponse>()
                .ToListAsync(cancellationToken);

            if (!orders.Any())
                return Result.Failure<List<OrderResponse>>(OrderErrors.NoOrdersFound);

            return Result.Success(orders);
        }

        public async Task<Result> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus, CancellationToken cancellationToken = default)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
            if (order is null)
                return Result.Failure(OrderErrors.NotFound);

            order.Status = newStatus;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}