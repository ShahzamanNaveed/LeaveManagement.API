using LeaveManagement.API.Common.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveManagement.API.Features.Management.Dtos;
using LeaveManagement.API.Features.Administration.Dtos;
using LeaveManagement.API.Features.Administration.Interfaces;


namespace LeaveManagement.API.Features.Administration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController : ControllerBase
    {

        private readonly IAdminService _adminService;


        public AdminController(
            IAdminService adminService)
        {
            _adminService = adminService;
        }



        // =========================
        // View Employees
        // =========================

        [HttpGet("employees")]
        [HasPermission("Employee.View")]
        public async Task<IActionResult> GetEmployees()
        {

            var employees =
                await _adminService
                .GetAllEmployeesAsync();


            return Ok(employees);

        }



        // =========================
        // Create Employee
        // =========================

        [HttpPost("employees")]
        [HasPermission("Employee.Create")]
        public async Task<IActionResult> CreateEmployee(
            CreateEmployeeRequestDto request)
        {
            await _adminService
                .CreateEmployeeAsync(request);


            return Ok(new
            {
                Message = "Employee created successfully."
            });
        }





        // =========================
        // Create Manager
        // =========================

        [HttpPost("managers")]
        [HasPermission("Manager.Create")]
        public async Task<IActionResult> CreateManager(
            CreateManagerRequestDto request)
        {

            await _adminService
                .CreateManagerAsync(request);


            return Ok(new
            {
                Message = "Manager created successfully."
            });

        }





        // =========================
        // Assign Manager
        // =========================

        [HttpPut("employees/{employeeId}/manager")]
        [HasPermission("Employee.AssignManager")]
        public async Task<IActionResult> AssignManager(
            int employeeId,
            AssignManagerRequestDto request)
        {

            await _adminService
                .AssignManagerAsync(
                    employeeId,
                    request);


            return Ok(new
            {
                Message = "Manager assigned successfully."
            });
        }

    }
}