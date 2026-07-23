using LeaveMangement.API.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace LeaveManagement.API.Authorization
{
    public class PermissionPolicyProvider
        : DefaultAuthorizationPolicyProvider
    {

        public PermissionPolicyProvider(
            IOptions<AuthorizationOptions> options)
            : base(options)
        {

        }



        public override async Task<AuthorizationPolicy?> GetPolicyAsync(
            string policyName)
        {

            if (policyName.StartsWith("Permission"))
            {
                var permission =
                    policyName.Replace(
                        "Permission:",
                        "");


                var policy =
                    new AuthorizationPolicyBuilder()
                    .AddRequirements(
                        new PermissionRequirement(permission))
                    .Build();


                return policy;
            }


            return await base.GetPolicyAsync(policyName);
        }

    }
}