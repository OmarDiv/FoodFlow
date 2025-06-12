namespace FoodFlow.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }       // الكمية (مثلاً: 2 برجر)
        public decimal Price { get; set; }      // سعر الوحدة وقت الطلب

        public Order Order { get; set; } = null!; // الطلب اللي ينتمي له هذا العنصر
        public MenuItem MenuItem { get; set; } = null!; // الطبق الذي تم طلبه
    }
}