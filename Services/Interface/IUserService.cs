using Microsoft.EntityFrameworkCore;

namespace FoodFlow.Services.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<UserResponse>> GetAsync(string id);
        Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken);
        Task<Result> UpdateAsync(string userId, UpdateUserRequest request, CancellationToken cancellationToken);
        Task<Result<UserProfileResponse>> GetUserProfileAsync(string userId);
        Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
        Task<Result> ToggleStatusAsync(string userId, CancellationToken cancellationToken);
        Task<Result> UnlockAsync(string userId, CancellationToken cancellationToken);
        Task<Result> SendChangeEmailCodeAsync(string userId, ChangeEmailRequest request);
        Task<Result> ConfirmChangeEmailAsync(ConfirmChangeEmailRequest request);
        Task<Result> ReSendChangeEmailCodeAsync(string userId, ReSendChangeEmailCode request);
        Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
        Task<Result> ConfirmSendEmailCodeAndSetPassowrd(ConfirmEmailAndSetPasswordRequest request);
        Task<Result> ResendConfirmSendEmailCodeAndSetPassowrd(ResendConfirmEmailAndSetPasswordRequest request, CancellationToken cancellationToken);
    }
}
