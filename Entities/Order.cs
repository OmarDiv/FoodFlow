namespace FoodFlow.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }     // العميل اللي عمل الطلب
        public int RestaurantId { get; set; }   // المطعم اللي تم الطلب منه
        public string DeliveryAddress { get; set; } // عنوان التوصيل
        public decimal TotalAmount { get; set; }    // إجمالي الطلب
                                                    // public OrderStatus Status { get; set; }     // حالة الطلب (معلق، جاري التحضير، الخ)
        public DateTime CreatedAt { get; set; }     // وقت الطلب

        //public User Customer { get; set; }
        public Restaurant Restaurant { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();  // تفاصيل الأطباق في الطلب
    }
}