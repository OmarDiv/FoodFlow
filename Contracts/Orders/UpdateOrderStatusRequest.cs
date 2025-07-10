using FoodFlow.Const.Enum;

namespace FoodFlow.Contracts.Orders
{
    public record UpdateOrderStatusRequest(
    OrderStatus NewStatus);
}
