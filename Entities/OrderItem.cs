namespace FoodFlow.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        // الطلب الرئيسي
        public int OrderId { get; set; }

        // عنصر الطعام
        public int MenuItemId { get; set; }

        // الكمية المطلوبة
        public int Quantity { get; set; }

        // السعر وقت الطلب (ممكن يتغير بعدين في المنيو)
        public decimal UnitPrice { get; set; }

        // إجمالي السعر لهذا العنصر (UnitPrice * Quantity)
        public decimal TotalPrice => UnitPrice * Quantity;
        public MenuItem MenuItem { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }
}