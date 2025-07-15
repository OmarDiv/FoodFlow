namespace FoodFlow.Errors
{
    public static class UserErrors
    {
        public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid email/password" ,StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidUserOrRefershToken = new("User.InvalidUserOrRefershToken", "Invalid user or refresh token.", StatusCodes.Status401Unauthorized);
        public static readonly Error FailedToUpdateUser = new("User.FailedToUpdateUser", "Failed to update user information.", StatusCodes.Status401Unauthorized);

    }
}
