namespace FoodFlow.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // مثلاً: "برجر دجاج"
        public string Description { get; set; } = string.Empty; // "برجر دجاج مشوي مع خضروات"
        public decimal Price { get; set; }      // 50 جنيه
        public string ImageUrl { get; set; } = string.Empty;   // صورة البرجر
        public bool IsAvailable { get; set; } = true;  // متاح للطلب أم لا
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;// "سندوتشات"

        //public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // الطلبات اللي تحتوي على هذا الطبق

    }
}