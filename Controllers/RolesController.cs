using FoodFlow.Abstractions;
using FoodFlow.Abstractions.Consts;
using FoodFlow.Abstractions.Filters;
using FoodFlow.Contracts.Roles;

namespace FoodFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRoleService roleService) : ControllerBase
    {
        private readonly IRoleService _roleService = roleService;

        [HttpGet("")]
        [HasPermission(Permissions.ViewRoles)]
        public async Task<IActionResult> GetAll([FromQuery] bool IncludeDisabled, CancellationToken cancellationToken)
        {
            var result = await _roleService.GetAllRolesAsync(IncludeDisabled, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [HasPermission(Permissions.ViewRoles)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var result = await _roleService.GetAsync(id);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost("")]
        [HasPermission(Permissions.CreateRole)]
        public async Task<IActionResult> Add([FromBody] RoleRequest request)
        {
            // Assuming you have a method to create a role
            var result = await _roleService.AddRoleAsync(request);

            return result.IsSuccess ? CreatedAtAction(nameof(Get), new { Id = result.Value!.Id }, result.Value) : result.ToProblem();

        }
        [HttpPut("{id}")]
        [HasPermission(Permissions.UpdateRole)]
        public async Task<IActionResult> Update(string id, [FromBody] RoleRequest request)
        {
            var result = await _roleService.UpdateRoleAsync(id, request);

            return result.IsSuccess ? NoContent() : result.ToProblem();

        }
        [HttpPut("{id}/toggle-status")]
        [HasPermission(Permissions.UpdateRole)]

        public async Task<IActionResult> ToggleStatus(string id)
        {
            var result = await _roleService.ToggleStatusAsync(id);

            return result.IsSuccess ? NoContent() : result.ToProblem();

        }

    }
}
