namespace FoodFlow.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;

        public MenuItem MenuItem { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }
}