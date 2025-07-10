//using FoodFlow.Const.Enum;
//using FoodFlow.Contracts.Orders;
//namespace FoodFlow.Services.Impelement
//{
//    public class OrderService(ApplicationDbContext dbContext) : IOrderService
//    {
//        private readonly ApplicationDbContext _dbContext = dbContext;

//        public async Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderRequest request, string userId)
//        {
//            try
//            {
//                var newOrder = request.Adapt<Order>();
//                newOrder.UserId = userId;
//                newOrder.Status = OrderStatus.Pending; // أو الحالة الافتراضية عندك

//                await _dbContext.Orders.AddAsync(newOrder);
//                await _dbContext.SaveChangesAsync();

//                var response = newOrder.Adapt<OrderResponse>();
//                return Result.Success(response);
//            }
//            catch
//            {
//                return Result.Failure<OrderResponse>(OrderErrors.FailedToCreate);
//            }
//        }

//        public async Task<Result<OrderResponse>> GetOrderByIdAsync(int orderId, string userId)
//        {
//            var order = await _dbContext.Orders
//                .AsNoTracking()
//                .FirstOrDefaultAsync(o => o.Id == orderId);

//            if (order is null)
//                return Result.Failure<OrderResponse>(OrderErrors.NotFound);

//            if (order.UserId != userId)
//                return Result.Failure<OrderResponse>(OrderErrors.Unauthorized);

//            var response = order.Adapt<OrderResponse>();
//            return Result.Success(response);
//        }

//        public async Task<Result<List<OrderResponse>>> GetOrdersForCustomerAsync(string userId)
//        {
//            var orders = await _dbContext.Orders
//                .Where(o => o.UserId == userId)
//                .AsNoTracking()
//                .ToListAsync();

//            if (!orders.Any())
//                return Result.Failure<List<OrderResponse>>(OrderErrors.NoOrdersFound);

//            var response = orders.Adapt<List<OrderResponse>>();
//            return Result.Success(response);
//        }

//        public async Task<Result<List<OrderResponse>>> GetOrdersForRestaurantAsync(int restaurantId)
//        {
//            var orders = await _dbContext.Orders
//                .Where(o => o.RestaurantId == restaurantId)
//                .AsNoTracking()
//                .ToListAsync();

//            if (!orders.Any())
//                return Result.Failure<List<OrderResponse>>(OrderErrors.NoOrdersFound);

//            var response = orders.Adapt<List<OrderResponse>>();
//            return Result.Success(response);
//        }

//        public async Task<Result<List<OrderResponse>>> GetPendingOrdersForRestaurantAsync(int restaurantId)
//        {
//            var orders = await _dbContext.Orders
//                .Where(o => o.RestaurantId == restaurantId && o.Status == OrderStatus.Pending)
//                .AsNoTracking()
//                .ToListAsync();

//            if (!orders.Any())
//                return Result.Failure<List<OrderResponse>>(OrderErrors.NoOrdersFound);

//            var response = orders.Adapt<List<OrderResponse>>();
//            return Result.Success(response);
//        }

//        public async Task<Result> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusRequest request)
//        {
//            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

//            if (order is null)
//                return Result.Failure(OrderErrors.NotFound);

//            // تحقق من صحة الحالة الجديدة (مثلاً: لا يمكن إلغاء طلب مكتمل)
//            if (!Enum.IsDefined(typeof(OrderStatus), request.Status))
//                return Result.Failure(OrderErrors.InvalidStatus);

//            order.Status = request.Status;

//            try
//            {
//                await _dbContext.SaveChangesAsync();
//                return Result.Success();
//            }
//            catch
//            {
//                return Result.Failure(OrderErrors.FailedToUpdate);
//            }
//        }

//        public async Task<Result> CancelOrderAsync(int orderId, string userId)
//        {
//            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

//            if (order is null)
//                return Result.Failure(OrderErrors.NotFound);

//            if (order.UserId != userId)
//                return Result.Failure(OrderErrors.Unauthorized);

//            if (order.Status == OrderStatus.Cancelled)
//                return Result.Failure(OrderErrors.AlreadyCancelled);

//            order.Status = OrderStatus.Cancelled;

//            try
//            {
//                await _dbContext.SaveChangesAsync();
//                return Result.Success();
//            }
//            catch
//            {
//                return Result.Failure(OrderErrors.FailedToCancel);
//            }
//        }
//    }
//}