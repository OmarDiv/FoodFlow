using FoodFlow.Abstractions.Consts;
using FoodFlow.Contracts.Roles;
using Microsoft.EntityFrameworkCore;

namespace FoodFlow.Services.Impelement
{
    public class RoleService(RoleManager<ApplicationRole> roleManager, ApplicationDbContext applicationDbContext) : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly ApplicationDbContext _context = applicationDbContext;

        public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync(bool? IncludeDisabled = false, CancellationToken cancellation = default) =>
            await _roleManager.Roles.Where(r => !r.IsDefault && (!r.IsDeleted || IncludeDisabled.HasValue && IncludeDisabled.Value))
            .ProjectToType<RoleResponse>()
                .AsNoTracking()
                .ToListAsync(cancellation);


        public async Task<Result<RoleDetailResponse>> GetAsync(string roleId)
        {

            if (await _roleManager.FindByIdAsync(roleId) is not { } role)
                return Result.Failure<RoleDetailResponse>(RoleErrors.NotFound);
            var permissions = await _roleManager.GetClaimsAsync(role);
            var response = new RoleDetailResponse(role.Id, role.Name!, role.IsDeleted, permissions.Select(clam => clam.Value));

            return Result.Success(response);
        }

        public async Task<Result<RoleDetailResponse>> AddRoleAsync(RoleRequest request)
        {
            var roleExists = await _roleManager.RoleExistsAsync(request.Name);
            if (roleExists)
                return Result.Failure<RoleDetailResponse>(RoleErrors.RoleAlreadyExists);
            var allowedpermissions = Permissions.GetAllPermissions();

            if (request.Permissions.Except(allowedpermissions).Any())
                return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);

            var role = new ApplicationRole
            {
                Name = request.Name,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                var permissions = request.Permissions
                    .Select(x => new IdentityRoleClaim<string>
                    {
                        RoleId = role.Id,
                        ClaimType = Permissions.Type,
                        ClaimValue = x
                    }
                    );
                await _context.RoleClaims.AddRangeAsync(permissions);
                await _context.SaveChangesAsync();
                var response = new RoleDetailResponse(role.Id, role.Name, role.IsDeleted, request.Permissions);
                return Result.Success(response);
            }

            var errors = result.Errors.First();
            return Result.Failure<RoleDetailResponse>(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> UpdateRoleAsync(string id, RoleRequest request)
        {
            if (await _roleManager.FindByIdAsync(id) is not { } role)
                return Result.Failure<RoleDetailResponse>(RoleErrors.NotFound);

            var roleExists = await _roleManager.Roles.AnyAsync(r => r.Name == request.Name && r.Id != id);
            if (roleExists)
                return Result.Failure<RoleDetailResponse>(RoleErrors.RoleAlreadyExists);

            var allowedpermissions = Permissions.GetAllPermissions();

            if (request.Permissions.Except(allowedpermissions).Any())
                return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);

            role.Name = request.Name;
            var result = await _roleManager.UpdateAsync(role);



            if (result.Succeeded)
            {
                var currentpermissions = await _context.RoleClaims
                    .Where(r => r.RoleId == role.Id && r.ClaimType == Permissions.Type)
                    .Select(r => r.ClaimValue)
                    .ToListAsync();

                var newPermissions = request.Permissions.Except(currentpermissions).Select(x => new IdentityRoleClaim<string>
                {
                    RoleId = role.Id,
                    ClaimType = Permissions.Type,
                    ClaimValue = x
                });

                var removedPermissions = currentpermissions.Except(request.Permissions);

                await _context.RoleClaims
                .Where(r => r.RoleId == id && removedPermissions.Contains(r.ClaimValue))
                .ExecuteDeleteAsync();

                await _context.RoleClaims.AddRangeAsync(newPermissions);
                await _context.SaveChangesAsync();
                return Result.Success();
            }

            var errors = result.Errors.First();
            return Result.Failure<RoleDetailResponse>(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ToggleStatusAsync(string id)
        {
            if (await _roleManager.FindByIdAsync (id) is not { } role)
                return Result.Failure<RoleDetailResponse>(RoleErrors.NotFound);
            role.IsDeleted = !role.IsDeleted;
            await _roleManager.UpdateAsync(role);
            return Result.Success();
        }

    }
}