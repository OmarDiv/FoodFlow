namespace FoodFlow.Errors
{
    public static class UserErrors
    {
        public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidUserOrRefershToken = new("User.InvalidUserOrRefershToken", "Invalid user or refresh token.", StatusCodes.Status401Unauthorized);
        public static readonly Error FailedToUpdateUser = new("User.FailedToUpdateUser", "Failed to update user information.", StatusCodes.Status401Unauthorized);
        public static readonly Error DuplicatedEmail = new("User.DuplicatedEmail", "Another User With The Same Email Already Exsist.", StatusCodes.Status409Conflict);
        public static readonly Error EmailNotConfirmed = new("User.EmailNotConfirmed", "Email Is Not Confirmed.", StatusCodes.Status401Unauthorized);
        public static readonly Error InvaildCode = new("User.InvaildCode", "Code Is Invalid.", StatusCodes.Status401Unauthorized);
        public static readonly Error DuplicatedConfirmation = new("User.DuplicatedConfirmation", "This Email Already Confirmed.", StatusCodes.Status401Unauthorized);

    }
}
