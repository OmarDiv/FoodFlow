using FoodFlow.Contracts.Roles;

namespace FoodFlow.Services.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponse>> GetAllRolesAsync(bool? IncludeDisabled = false, CancellationToken cancellationToken = default);
        Task<Result<RoleDetailResponse>> GetAsync(string roleId);
        Task<Result<RoleDetailResponse>> AddRoleAsync(RoleRequest request);
        Task<Result> UpdateRoleAsync(string id, RoleRequest request);
        Task<Result> ToggleStatusAsync(string id);
    }
}
