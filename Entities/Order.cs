using FoodFlow.Const.Enum;

namespace FoodFlow.Entities
{
    public class Order : AuditableEntity
    {
        public int Id { get; set; }
        // رقم الطلب (Primary Key)

        public string DeliveryAddress { get; set; } = string.Empty;
        // عنوان التوصيل اللي العميل اختاره للطلب

        public decimal TotalAmount { get; set; }
        // إجمالي سعر الطلب (مجموع أسعار كل العناصر + رسوم التوصيل لو فيه)

        public OrderStatus Status { get; set; }
        // حالة الطلب (Pending, OnTheWay, Delivered, Cancelled)

        public DateTime CreatedAt { get; set; }
        // وقت إنشاء الطلب (مفيد للترتيب والتقارير)

        public string CustomerId { get; set; } = string.Empty;
        // رقم تعريف العميل اللي عمل الطلب (Foreign Key لـ ApplicationUser)

        public int RestaurantId { get; set; }
        // رقم المطعم اللي الطلب جاي منه (Foreign Key لـ Restaurant)

        public ApplicationUser Customer { get; set; } = null!;
        // الـ Object الخاص بالعميل (Navigation Property)

        public Restaurant Restaurant { get; set; } = null!;
        // الـ Object الخاص بالمطعم (Navigation Property)

        public ICollection<OrderItem> OrderItems { get; set; } = [];
        // قائمة العناصر (الأطباق) اللي في الطلب (Navigation Property)
    }

}