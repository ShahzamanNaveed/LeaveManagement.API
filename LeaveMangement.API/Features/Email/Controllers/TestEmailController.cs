using LeaveManagement.API.Features.Email.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Features.Email.Controllers
{
    [ApiController]
    [Route("api/test-email")]
    public class TestEmailController : ControllerBase
    {

        private readonly IEmailService _emailService;


        public TestEmailController(
            IEmailService emailService)
        {
            _emailService = emailService;
        }



        [HttpGet]
        public async Task<IActionResult> SendTestEmail()
        {

            await _emailService.SendEmailAsync(
                "shahzamannaveed247@gmail.com",
                "Leave Management Test Email",
                "SMTP email is working successfully."
            );


            return Ok(new
            {
                Message = "Test email sent successfully."
            });

        }

    }
}