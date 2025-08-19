using Microsoft.AspNetCore.Authorization;

namespace FoodFlow.Controllers
{
    [Route("me")]
    [ApiController]
    [Authorize]
    public class AccountController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        [HttpGet("")]
        public async Task<IActionResult> Info()
        {
            var result = await _userService.GetUserProfileAsync(User.GetUserId()!);
            return Ok(result.Value);
        }
        [HttpPut("info")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
        {
            var result = await _userService.UpdateProfileAsync(User.GetUserId()!, request);
            return Ok();
        }
        [HttpPost("change-email")]
        public async Task<IActionResult> SendChangeEmailCode(ChangeEmailRequest request)
        {
            var result = await _userService.SendChangeEmailCodeAsync(User.GetUserId()!, request);
            return result.IsSuccess ?
                Ok("Confirmation email sent. Please check your new email to confirm the change.")
                : result.ToProblem();
        }
        [HttpPost("confirm-change-email")]
        public async Task<IActionResult> ConfirmChangeEmail(ConfirmChangeEmailRequest request)
        {
            var result = await _userService.ConfirmChangeEmailAsync(request);
            return result.IsSuccess ? Ok("Email changed successfully.") : result.ToProblem();
        }
        [HttpPost("resend-confirm-change-email")]
        public async Task<IActionResult> ResndConfirmChangeEmail(ReSendChangeEmailCode request)
        {
            var result = await _userService.ReSendChangeEmailCodeAsync(User.GetUserId()!, request);
            return result.IsSuccess ? Ok("Confirmation email sent. Please check your new email to confirm the change.") : result.ToProblem();
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var result = await _userService.ChangePasswordAsync(User.GetUserId()!, request);

            return result.IsSuccess
                ? Ok("Password changed successfully.")
                : result.ToProblem();
        }

    }
}
