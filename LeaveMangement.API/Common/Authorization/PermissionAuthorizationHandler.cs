using LeaveManagement.API.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.API.Common.Authorization
{
    public class PermissionAuthorizationHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ApplicationDbContext _context;


        public PermissionAuthorizationHandler(
            ApplicationDbContext context)
        {
            _context = context;
        }


        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {

            var roleName =
                context.User.Claims
                .FirstOrDefault(c =>
                    c.Type ==
                    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                ?.Value;


            if (roleName == null)
            {
                return;
            }


            var hasPermission =
                await _context.RolePermissions
                .AnyAsync(rp =>
                    rp.Role.Name == roleName
                    &&
                    rp.Permission.Name == requirement.Permission);



            if (hasPermission)
            {
                context.Succeed(requirement);
            }

        }
    }
}