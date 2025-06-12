using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlow.Persistence.EntitesConfigurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.HasIndex(m => new { m.Name, m.CategoryId }).IsUnique();

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.ImageUrl)
                .HasMaxLength(300);

            builder.Property(m => m.Price)
                .HasPrecision(18, 2)
                .IsRequired();
        }
    }
}