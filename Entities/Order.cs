using FoodFlow.Const.Enum;

namespace FoodFlow.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty; // عنوان التوصيل
        public decimal TotalAmount { get; set; }    // إجمالي الطلب
        public OrderStatus Status { get; set; } = OrderStatus.pending;     // حالة الطلب (معلق، جاري التحضير، الخ)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;    // وقت الطلب
        public ApplicationUser Customer { get; set; } = null!;
        public Restaurant Restaurant { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } =[];  // تفاصيل الأطباق في الطلب
    }
}