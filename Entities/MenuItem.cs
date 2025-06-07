namespace FoodFlow.Entities
{
    public class MenuItem
    {

        public int Id { get; set; }
        public string Name { get; set; }  // مثلاً: "برجر دجاج"
        public string Description { get; set; } // "برجر دجاج مشوي مع خضروات"
        public decimal Price { get; set; }      // 50 جنيه
        public string Category { get; set; }    // "سندوتشات"
        public string ImageUrl { get; set; }    // صورة البرجر
        public Restaurant Restaurant { get; set; } // المطعم اللي بيقدم البرجر

        // public ICollection<OrderItem> OrderItems { get; set; }

    }
}