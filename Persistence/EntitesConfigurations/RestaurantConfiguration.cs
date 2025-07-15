using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlow.Persistence.EntitesConfigurations
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasIndex(r => r.PhoneNumber).IsUnique();
            builder.HasIndex(r => new { r.Name, r.Address }).IsUnique();

            builder.HasMany(c => c.Categories)
                          .WithOne(r => r.Restaurant)
                          .HasForeignKey(o => o.RestaurantId);

            builder.HasMany(c => c.Orders)
                          .WithOne(r => r.Restaurant)
                          .HasForeignKey(o => o.RestaurantId);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(r => r.Address)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.LogoUrl)
                .HasMaxLength(300);

            builder.Property(r => r.Description)
                .HasMaxLength(500);
        }
    }
}