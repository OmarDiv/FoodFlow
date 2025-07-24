namespace FoodFlow.Entities
{
    public class Category : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // مثال: "سندوتشات"
        public bool IsAvailable { get; set; } = true;
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;
        public ICollection<MenuItem> MenuItems { get; set; } = [];
    }
}