using FoodFlow.Abstractions.Consts;
using FoodFlow.Abstractions.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace FoodFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        [HasPermission(Permissions.ViewUsers)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllAsync(cancellationToken);

            return Ok(users);
        }

        [HttpGet("{id}")]
        [HasPermission(Permissions.ViewUsers)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var result = await _userService.GetAsync(id);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost("")]
        [HasPermission(Permissions.CreateUser)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.AddAsync(request, cancellationToken);
            return result.IsSuccess ? CreatedAtAction(nameof(Get), new { result.Value!.Id }, result.Value) : result.ToProblem();
        }
        [HttpPut("{id}")]
        [HasPermission(Permissions.UpdateUser)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.UpdateAsync(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPost("confirm-email-and-set-password")]
        public async Task<IActionResult> ConfirmEmailAndSetPassword([FromBody] ConfirmEmailAndSetPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.ConfirmSendEmailCodeAndSetPassowrd(request);

            return result.IsSuccess ? NoContent() : result.ToProblem();

        }
        [HttpPost("resend-confirm-email-and-set-password")]
        [HasPermission(Permissions.CreateUser)]
        public async Task<IActionResult> ReSendConfirmEmailAndSetPassword([FromBody] ResendConfirmEmailAndSetPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _userService.ResendConfirmSendEmailCodeAndSetPassowrd(request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();

        }
        [HttpPut("{id}/toggle-status")]
        [HasPermission(Permissions.UpdateUser)]
        public async Task<IActionResult> ToggleStatus([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            var result = await _userService.ToggleStatusAsync(id, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();

        }
        [HttpPut("{id}/unlock")]
        [HasPermission(Permissions.UpdateUser)]
        public async Task<IActionResult> Unlock([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            var result = await _userService.UnlockAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
