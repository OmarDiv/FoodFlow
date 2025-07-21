using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlow.Persistence.EntitesConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Properties
            builder.Property(o => o.DeliveryAddress)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(o => o.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status)
                .IsRequired();

            builder.Property(o => o.CustomerId)
                .IsRequired();

            builder.Property(o => o.RestaurantId)
                .IsRequired();

            builder.HasOne(u => u.CreatedBy)
                .WithMany()
                .HasForeignKey(u => u.CreatedById);
            builder.HasOne(u => u.UpdatedBy)
                .WithMany()
                .HasForeignKey(u => u.UpdatedById);

            builder.HasOne(r => r.Restaurant)
                .WithMany(o => o.Orders)
                .HasForeignKey(r => r.RestaurantId);

            builder.HasOne(r => r.Customer)
                .WithMany(o => o.Orders)
                .HasForeignKey(r => r.CustomerId);

            builder.HasMany(o => o.OrderItems)
                .WithOne(o => o.Order)
                .HasForeignKey(o => o.OrderId);
        }
    }
}
