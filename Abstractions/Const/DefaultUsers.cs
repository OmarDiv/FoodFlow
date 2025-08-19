namespace FoodFlow.Abstractions.Const
{
    public static class DefaultUsers
    {
        public const string AdminId = "D1C3829B-4448-4B56-B194-4AFA694CE3A3";
        public const string AdminEmail = "admin@food-flow.com";
        public const string AdminPassword = "P@ssword123";
        public const string AdminSecurityStamp = "F7D722E8A6C54C5FA10F498C09D2EF90";
        public const string AdminConcurrencyStamp = "01986241-4662-71f4-95b6-31abf6024769";
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByIdAsync(AdminId) == null)
            {
                var adminUser = new ApplicationUser
                {
                    Id = AdminId,
                    FirstName = "Food Flow",
                    LastName = "Admin",
                    UserName = AdminEmail,
                    NormalizedUserName = AdminEmail.ToUpper(),
                    Email = AdminEmail,
                    NormalizedEmail = AdminEmail.ToUpper(),
                    SecurityStamp = AdminSecurityStamp,
                    ConcurrencyStamp = AdminConcurrencyStamp,
                    EmailConfirmed = true
                };
                var user = await userManager.FindByEmailAsync(adminUser.Email);
                if (user is null)
                {
                    await userManager.CreateAsync(adminUser, AdminPassword);
                    await userManager.AddToRoleAsync(adminUser, DefaultRoles.Admin);
                }
            }
        }
    }
}
