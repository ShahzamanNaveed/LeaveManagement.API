using Microsoft.AspNetCore.Authorization;

namespace LeaveManagement.API.Common.Authorization
{
    public class PermissionRequirement
        : IAuthorizationRequirement
    {

        public string Permission { get; }


        public PermissionRequirement(
            string permission)
        {
            Permission = permission;
        }

    }
}