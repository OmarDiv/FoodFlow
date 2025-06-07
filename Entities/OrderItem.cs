namespace FoodFlow.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }        // الطلب اللي بيتبع له
        public int MenuItemId { get; set; }     // الطبق اللي تم طلبه
        public int Quantity { get; set; }       // الكمية (مثلاً: 2 برجر)
        public decimal Price { get; set; }      // سعر الوحدة وقت الطلب

        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}