using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FoodFlow.Entities;

namespace FoodFlow.Persistence.EntitesConfigurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Properties
            builder.Property(oi => oi.Quantity)
                .IsRequired();
            builder.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(o => o.OrderId);

            builder.HasOne(o => o.MenuItem)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(o => o.MenuItemId);
        }
    }
}
