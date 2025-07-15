namespace FoodFlow.Entities
{
    public class Order : AuditableEntity
    {
        public int Id { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public int RestaurantId { get; set; }
        public ApplicationUser Customer { get; set; } = null!;
        public Restaurant Restaurant { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = [];
    }
}