using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlow.Persistence.EntitesConfigurations
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasIndex(r => r.PhoneNumber).IsUnique();


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

            // Relationships
            //builder.HasOne(r => r.Owner)
            //    .WithMany(u => u.Restaurants)
            //    .HasForeignKey(r => r.OwnerId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // Unique constraints
        }

    }
}
