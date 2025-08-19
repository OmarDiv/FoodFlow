using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodFlow.Persistence.EntitesConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {

        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {

            builder.HasData([
                new ApplicationRole{
                    Id = DefaultRoles.AdminRoleId,
                    Name = DefaultRoles.Admin,
                    NormalizedName = DefaultRoles.Admin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp
                }
                ,
                new ApplicationRole{
                    Id = DefaultRoles.CustomerRoleId,
                    Name = DefaultRoles.Customer,
                    NormalizedName = DefaultRoles.Customer.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.CustomerRoleConcurrencyStamp,
                    IsDefault = true
                }
                ,
                new ApplicationRole{
                    Id = DefaultRoles.RestaurantOwnerRoleId,
                    Name = DefaultRoles.RestaurantOwner,
                    NormalizedName = DefaultRoles.RestaurantOwner.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.RestaurantOwnerRoleConcurrencyStamp
                }
                ,
                new ApplicationRole{
                    Id = DefaultRoles.DeliveryRoleId,
                    Name = DefaultRoles.Delivery,
                    NormalizedName = DefaultRoles.Delivery.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.DeliveryRoleConcurrencyStamp
                }
                ]);
        }
    }
}
