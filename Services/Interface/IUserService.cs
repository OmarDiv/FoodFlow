namespace FoodFlow.Services.Interface
{
    public interface IUserService
    {
        Task<Result<UserProfileResponse>> GetUserProfileAsync(string userId);
        Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
        Task<Result> SendChangeEmailCodeAsync(string userId, ChangeEmailRequest request);
        Task<Result> ConfirmChangeEmailAsync(ConfirmChangeEmailRequest request);
        Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
       
    }
}
