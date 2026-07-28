using Microsoft.AspNetCore.Authorization;

namespace LeaveManagement.API.Common.Authorization
{
    public class HasPermissionAttribute
        : AuthorizeAttribute
    {

        public HasPermissionAttribute(
            string permission)
        {
            Policy = $"Permission:{permission}";
        }

    }
}