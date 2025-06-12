using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlow.Persistence.EntitesConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasIndex(c => new { c.Name, c.RestaurantId }).IsUnique();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}