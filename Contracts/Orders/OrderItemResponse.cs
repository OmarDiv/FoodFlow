namespace FoodFlow.Contracts.Orders
{
    public record OrderItemResponse(
    int MenuItemId,
    string MenuItemName,
    int Quantity,
    decimal UnitPrice);
}
