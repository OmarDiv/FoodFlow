namespace FoodFlow.Entities
{
    public class Restaurant : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsOpen { get; set; }

        public ICollection<Category> Categories { get; set; } = [];
        //public ICollection<Order> Orders { get; set; }
    }
}