namespace FoodFlow.Contracts.Orders
{
    public record OrderResponse(
    int Id,
    string Status,
    decimal TotalAmount,
    DateTime CreatedAt,
    List<OrderItemResponse> Items);
}
