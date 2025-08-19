namespace FoodFlow.Abstractions.Const
{
    public static class UserManagerExtensions
    {
        public static async Task SeedAdminUserAsync(this UserManager<ApplicationUser> userManager)
        {
            var adminUser = await userManager.FindByEmailAsync(DefaultUsers.AdminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    Id = DefaultUsers.AdminId,
                    FirstName = "Food Flow",
                    LastName = "Admin",
                    UserName = DefaultUsers.AdminEmail,
                    NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
                    Email = DefaultUsers.AdminEmail,
                    NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
                    SecurityStamp = DefaultUsers.AdminSecurityStamp,
                    ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, DefaultUsers.AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, DefaultRoles.Admin);
                }
            }
            else
            {
                // تأكد أنه في الرول
                var roles = await userManager.GetRolesAsync(adminUser);
                if (!roles.Contains(DefaultRoles.Admin))
                {
                    await userManager.AddToRoleAsync(adminUser, DefaultRoles.Admin);
                }
            }
        }
    }
}