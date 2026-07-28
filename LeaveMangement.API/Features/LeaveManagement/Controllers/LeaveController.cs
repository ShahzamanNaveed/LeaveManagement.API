using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveManagement.API.Common.Authorization;
using LeaveManagement.API.Features.LeaveManagement.Dtos;
using LeaveManagement.API.Features.LeaveManagement.Interfaces;

namespace LeaveManagement.API.Features.LeaveManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;


        public LeaveController(
            ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }



        // =========================
        // Employee Apply Leave
        // =========================

        [HttpPost("apply")]
        [HasPermission("Leave.Apply")]
        public async Task<IActionResult> ApplyLeave(
            ApplyLeaveRequestDto request)
        {
            var employeeIdClaim =
                User.FindFirst("EmployeeId");


            if (employeeIdClaim == null)
            {
                return Unauthorized(new
                {
                    Message = "Employee ID not found in token."
                });
            }


            int employeeId =
                int.Parse(employeeIdClaim.Value);



            await _leaveService.ApplyLeaveAsync(
                employeeId,
                request);



            return Ok(new
            {
                Message = "Leave applied successfully."
            });
        }





        // =========================
        // Employee Leave History
        // =========================

        [HttpGet("my-leaves")]
        [HasPermission("Leave.ViewOwn")]
        public async Task<IActionResult> GetMyLeaves()
        {
            var employeeIdClaim =
                User.FindFirst("EmployeeId");


            if (employeeIdClaim == null)
            {
                return Unauthorized(new
                {
                    Message = "Employee ID not found in token."
                });
            }


            int employeeId =
                int.Parse(employeeIdClaim.Value);



            var leaves =
                await _leaveService.GetMyLeavesAsync(
                    employeeId);



            return Ok(leaves);
        }





        // =========================
        // Employee Leave Balance
        // =========================

        [HttpGet("my-balances")]
        [HasPermission("Leave.ViewBalance")]
        public async Task<IActionResult> GetMyBalances()
        {
            var employeeIdClaim =
                User.FindFirst("EmployeeId");


            if (employeeIdClaim == null)
            {
                return Unauthorized(new
                {
                    Message = "Employee ID not found in token."
                });
            }


            int employeeId =
                int.Parse(employeeIdClaim.Value);



            var balances =
                await _leaveService.GetMyBalancesAsync(
                    employeeId);



            return Ok(balances);
        }





        // =========================
        // Employee Cancel Leave
        // =========================

        [HttpPut("cancel/{leaveRequestId}")]
        [HasPermission("Leave.Cancel")]
        public async Task<IActionResult> CancelLeave(
            int leaveRequestId)
        {
            var employeeIdClaim =
                User.FindFirst("EmployeeId");


            if (employeeIdClaim == null)
            {
                return Unauthorized(new
                {
                    Message = "Employee ID not found in token."
                });
            }


            int employeeId =
                int.Parse(employeeIdClaim.Value);



            await _leaveService.CancelLeaveAsync(
                leaveRequestId,
                employeeId);



            return Ok(new
            {
                Message = "Leave cancelled successfully."
            });
        }





        // =========================
        // Manager Pending Leaves
        // =========================

        [HttpGet("pending")]
        [HasPermission("Leave.ViewPending")]
        public async Task<IActionResult> GetPendingRequestsForManager()
        {
            var managerIdClaim =
                User.FindFirst("EmployeeId");


            if (managerIdClaim == null)
            {
                return Unauthorized(new
                {
                    Message = "Manager ID not found in token."
                });
            }


            int managerId =
                int.Parse(managerIdClaim.Value);



            var requests =
                await _leaveService
                .GetPendingRequestsForManagerAsync(
                    managerId);



            return Ok(requests);
        }





        // =========================
        // Manager Approve Leave
        // =========================

        [HttpPut("approve/{leaveRequestId}")]
        [HasPermission("Leave.Approve")]
        public async Task<IActionResult> ApproveLeave(
            int leaveRequestId)
        {
            var managerIdClaim =
                User.FindFirst("EmployeeId");


            if (managerIdClaim == null)
            {
                return Unauthorized(new
                {
                    Message = "Manager ID not found in token."
                });
            }


            int managerId =
                int.Parse(managerIdClaim.Value);



            await _leaveService.ApproveLeaveAsync(
                leaveRequestId,
                managerId);



            return Ok(new
            {
                Message = "Leave approved successfully."
            });
        }




        // =========================
        // Manager Reject Leave
        // =========================

        [HttpPut("reject/{leaveRequestId}")]
        [HasPermission("Leave.Reject")]
        public async Task<IActionResult> RejectLeave(
            int leaveRequestId)
        {
            var managerIdClaim =
                User.FindFirst("EmployeeId");


            if (managerIdClaim == null)
            {
                return Unauthorized(new
                {
                    Message = "Manager ID not found in token."
                });
            }


            int managerId =
                int.Parse(managerIdClaim.Value);



            await _leaveService.RejectLeaveAsync(
                leaveRequestId,
                managerId);



            return Ok(new
            {
                Message = "Leave rejected successfully."
            });
        }
    }
}