namespace FoodFlow.Abstractions.Const
{
    public static class DefaultRoles
    {
        public const string Admin = nameof(Admin);
        public const string AdminRoleId = "01986241-4662-71f4-95b6-31ac4820553f";
        public const string AdminRoleConcurrencyStamp = "0198680f-89d0-7335-8307-a348cee60de4";

        public const string Customer = nameof(Customer);
        public const string CustomerRoleId = "01986241-4662-71f4-95b6-31b2d584c3ad";
        public const string CustomerRoleConcurrencyStamp = "0198680f-89d0-7335-8307-a347826a8c88";

        public const string Delivery = nameof(Delivery);
        public const string DeliveryRoleId = "01986241-4662-71f4-95b6-31bb2a547123";
        public const string DeliveryRoleConcurrencyStamp = "0198680f-89d0-7335-8307-a333865a77f1";

        public const string RestaurantOwner = nameof(RestaurantOwner);
        public const string RestaurantOwnerRoleId = "01986241-4662-71f4-95b6-31ab855ae411";
        public const string RestaurantOwnerRoleConcurrencyStamp = "0198680f-89d0-7335-8307-a378defa9109";
    }
}
