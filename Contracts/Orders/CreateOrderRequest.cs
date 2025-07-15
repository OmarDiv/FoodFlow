namespace FoodFlow.Contracts.Orders
{
    public record CreateOrderRequest(
        int RestaurantId,
        int CustomerAddressId,
        List<OrderItemDto> Items
    );
}
