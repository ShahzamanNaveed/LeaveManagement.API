using LeaveManagement.API.Common.Authorization;
using LeaveManagement.API.Features.FiscalYears.Dtos;
using LeaveManagement.API.Features.FiscalYears.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Features.FiscalYears.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FiscalYearSettingsController : ControllerBase
    {
        private readonly IFiscalYearSettingsService _service;

        public FiscalYearSettingsController(
            IFiscalYearSettingsService service)
        {
            _service = service;
        }

        // ==========================================
        // Get Fiscal Year Settings
        // ==========================================

        [HttpGet]
        [HasPermission("Employee.View")]
        public async Task<ActionResult<FiscalYearSettingsResponseDto>>
            Get()
        {
            var result =
                await _service.GetAsync();

            return Ok(result);
        }

        // ==========================================
        // Create Fiscal Year Settings
        // ==========================================

        [HttpPost]
        [HasPermission("Employee.Create")]
        public async Task<ActionResult<FiscalYearSettingsResponseDto>>
            Create(
                CreateFiscalYearSettingsDto request)
        {
            var result =
                await _service.CreateAsync(request);

            return Ok(result);
        }

        // ==========================================
        // Update Fiscal Year Settings
        // ==========================================

        [HttpPut]
        [HasPermission("Employee.Update")]
        public async Task<ActionResult<FiscalYearSettingsResponseDto>>
            Update(
                CreateFiscalYearSettingsDto request)
        {
            var result =
                await _service.UpdateAsync(request);

            return Ok(result);
        }
    }
}