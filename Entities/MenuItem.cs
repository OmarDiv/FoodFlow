namespace FoodFlow.Entities
{
    public class MenuItem : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 
        public decimal Price { get; set; }     
        public string ImageUrl { get; set; } = string.Empty;   
        public bool IsAvailable { get; set; } = true;  
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); 

    }
}