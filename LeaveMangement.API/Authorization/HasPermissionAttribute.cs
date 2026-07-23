using Microsoft.AspNetCore.Authorization;

namespace LeaveManagement.API.Authorization
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