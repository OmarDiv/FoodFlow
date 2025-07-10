namespace FoodFlow.Const
{
    public static class UserErrors
    {
        public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid email/password");
        public static readonly Error InvalidUserOrRefershToken = new("User.InvalidUserOrRefershToken", "Invalid user or refresh token.");
        public static readonly Error FailedToUpdateUser = new("User.FailedToUpdateUser", "Failed to update user information.");

    }
}
