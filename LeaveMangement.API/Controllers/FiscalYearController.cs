using LeaveManagement.API.Authorization;
using LeaveMangement.API.DTOs;
using LeaveMangement.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FiscalYearController : ControllerBase
    {
        private readonly IFiscalYearManagementService _service;


        public FiscalYearController(
            IFiscalYearManagementService service)
        {
            _service = service;
        }



        // ==========================================
        // Get All Fiscal Years
        // ==========================================

        [HttpGet]
        [HasPermission("Employee.View")]
        public async Task<ActionResult<List<FiscalYearResponseDto>>>
            GetAll()
        {
            var result =
                await _service.GetAllAsync();


            return Ok(result);
        }




        // ==========================================
        // Create Fiscal Year Manually
        // ==========================================

        [HttpPost]
        [HasPermission("Employee.Create")]
        public async Task<ActionResult<FiscalYearResponseDto>>
            Create(
                CreateFiscalYearDto request)
        {
            var result =
                await _service.CreateAsync(request);


            return Ok(result);
        }




        // ==========================================
        // Activate Fiscal Year
        // ==========================================

        [HttpPut("{id}/activate")]
        [HasPermission("Employee.Create")]
        public async Task<IActionResult>
            Activate(
                int id)
        {
            await _service.ActivateAsync(id);


            return Ok(new
            {
                Message =
                    "Fiscal year activated successfully."
            });
        }




        // ==========================================
        // Generate Next Fiscal Year Automatically
        // ==========================================

        [HttpPost("generate-next")]
        [HasPermission("Employee.Create")]
        public async Task<ActionResult<FiscalYearResponseDto>>
            GenerateNext()
        {
            var result =
                await _service.GenerateNextAsync();


            return Ok(result);
        }
    }
}