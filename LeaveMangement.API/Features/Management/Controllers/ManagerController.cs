using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Features.Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    public class ManagerController : ControllerBase
    {

        // Future manager-specific features:
        //
        // Team dashboard
        // Team members
        // Team leave calendar
        // Manager reports

    }
}