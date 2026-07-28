using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Features.Employees.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetProfile()
        {
            return Ok(new
            {
                Message = "You are authenticated.",
                User = User.Identity?.Name
            });
        }
    }
}