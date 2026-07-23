using LeaveManagement.API.Enums;
using LeaveManagement.API.Exceptions;
using LeaveMangement.API.DTOs;
using LeaveManagement.API.Models;
using LeaveMangement.API.Interfaces;

namespace LeaveMangement.API.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        private readonly ILeaveBalanceRepository _leaveBalanceRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IEmailService _emailService;



        public LeaveService(
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveBalanceRepository leaveBalanceRepository,
            IEmployeeRepository employeeRepository,
            IEmailService emailService)
        {
            _leaveRequestRepository =
                leaveRequestRepository;

            _leaveBalanceRepository =
                leaveBalanceRepository;

            _employeeRepository =
                employeeRepository;

            _emailService =
                emailService;
        }





        public async Task ApplyLeaveAsync(
            int employeeId,
            ApplyLeaveRequestDto request)
        {

            if (request.StartDate > request.EndDate)
            {
                throw new BadRequestException(
                    "Start date cannot be after end date.");
            }



            bool hasPending =
                await _leaveRequestRepository
                .HasPendingRequestAsync(employeeId);



            if (hasPending)
            {
                throw new BadRequestException(
                    "You already have a pending leave request.");
            }




            bool hasOverlap =
                await _leaveRequestRepository
                .HasOverlappingRequestAsync(
                    employeeId,
                    request.StartDate,
                    request.EndDate);



            if (hasOverlap)
            {
                throw new BadRequestException(
                    "Leave dates overlap with existing request.");
            }




            double numberOfDays;


            if (request.IsHalfDay)
            {
                numberOfDays = 0.5;
            }
            else
            {
                numberOfDays =
                    CalculateBusinessDays(
                        request.StartDate,
                        request.EndDate);
            }




            var balance =
                await _leaveBalanceRepository
                .GetBalanceAsync(
                    employeeId,
                    request.LeaveType,
                    DateTime.UtcNow.Year);



            if (balance == null)
            {
                throw new NotFoundException(
                    "Leave balance not found.");
            }




            if (balance.RemainingBalance < numberOfDays)
            {
                throw new BadRequestException(
                    "Insufficient leave balance.");
            }





            var leaveRequest =
                new LeaveRequest
                {
                    EmployeeId = employeeId,

                    LeaveType = request.LeaveType,

                    StartDate = request.StartDate,

                    EndDate = request.EndDate,

                    NumberOfDays = numberOfDays,

                    IsHalfDay = request.IsHalfDay,

                    Reason = request.Reason,

                    Status = LeaveStatus.Submitted,

                    AppliedAt = DateTime.UtcNow
                };





            await _leaveRequestRepository
                .AddAsync(leaveRequest);



            await _leaveRequestRepository
                .SaveChangesAsync();





            // ===============================
            // Send Email To Manager
            // ===============================


            var employee =
                await _employeeRepository
                .GetEmployeeWithManagerAsync(employeeId);



            if (employee != null &&
                employee.Manager != null &&
                employee.Manager.User != null)
            {

                string emailBody =
                    $"""
                    New Leave Request Submitted

                    Employee:
                    {employee.FullName}

                    Department:
                    {employee.Department}

                    Leave Type:
                    {request.LeaveType}

                    Start Date:
                    {request.StartDate:dd-MM-yyyy}

                    End Date:
                    {request.EndDate:dd-MM-yyyy}

                    Number Of Days:
                    {numberOfDays}

                    Reason:
                    {request.Reason}

                    Please review the request.
                    """;



                await _emailService
                    .SendEmailAsync(
                        employee.Manager.User.Email!,
                        "New Leave Request Submitted",
                        emailBody);
            }

        }






        public async Task<List<LeaveResponseDto>>
            GetMyLeavesAsync(
                int employeeId)
        {

            var leaves =
                await _leaveRequestRepository
                .GetEmployeeLeavesAsync(employeeId);



            return leaves.Select(l =>
                new LeaveResponseDto
                {
                    Id = l.Id,

                    LeaveType =
                        l.LeaveType.ToString(),

                    StartDate =
                        l.StartDate,

                    EndDate =
                        l.EndDate,

                    NumberOfDays =
                        l.NumberOfDays,

                    IsHalfDay =
                        l.IsHalfDay,

                    Reason =
                        l.Reason,

                    Status =
                        l.Status.ToString(),

                    AppliedAt =
                        l.AppliedAt

                }).ToList();
        }





        public async Task<List<LeaveBalanceResponseDto>>
            GetMyBalancesAsync(
                int employeeId)
        {

            var balances =
                await _leaveBalanceRepository
                .GetEmployeeBalancesAsync(employeeId);



            return balances.Select(b =>
                new LeaveBalanceResponseDto
                {
                    LeaveType =
                        b.LeaveType.ToString(),

                    TotalBalance =
                        b.TotalBalance,

                    ConsumedBalance =
                        b.ConsumedBalance,

                    RemainingBalance =
                        b.RemainingBalance,

                    Year =
                        b.Year

                }).ToList();

        }
        public async Task<List<ManagerLeaveResponseDto>>
    GetPendingRequestsForManagerAsync(
        int managerId)
        {

            var requests =
                await _leaveRequestRepository
                .GetPendingRequestsForManagerAsync(managerId);



            return requests.Select(l =>
                new ManagerLeaveResponseDto
                {
                    Id = l.Id,

                    EmployeeName =
                        l.Employee.FullName,

                    LeaveType =
                        l.LeaveType.ToString(),

                    StartDate =
                        l.StartDate,

                    EndDate =
                        l.EndDate,

                    NumberOfDays =
                        l.NumberOfDays,

                    IsHalfDay =
                        l.IsHalfDay,

                    Reason =
                        l.Reason,

                    Status =
                        l.Status.ToString(),

                    AppliedAt =
                        l.AppliedAt

                }).ToList();
        }






        public async Task ApproveLeaveAsync(
            int leaveRequestId,
            int managerId)
        {

            var leaveRequest =
                await _leaveRequestRepository
                .GetByIdAsync(leaveRequestId);



            if (leaveRequest == null)
            {
                throw new NotFoundException(
                    "Leave request not found.");
            }




            if (leaveRequest.Employee.ManagerId != managerId)
            {
                throw new UnauthorizedException(
                    "You cannot approve this leave.");
            }





            if (leaveRequest.Status != LeaveStatus.Submitted)
            {
                throw new BadRequestException(
                    "Only submitted leave requests can be approved.");
            }





            var balance =
                await _leaveBalanceRepository
                .GetBalanceAsync(
                    leaveRequest.EmployeeId,
                    leaveRequest.LeaveType,
                    DateTime.UtcNow.Year);





            if (balance == null)
            {
                throw new NotFoundException(
                    "Leave balance not found.");
            }





            if (balance.RemainingBalance <
                leaveRequest.NumberOfDays)
            {
                throw new BadRequestException(
                    "Insufficient leave balance.");
            }





            balance.ConsumedBalance +=
                leaveRequest.NumberOfDays;


            balance.RemainingBalance -=
                leaveRequest.NumberOfDays;



            leaveRequest.Status =
                LeaveStatus.Approved;


            leaveRequest.ApprovedByEmployeeId =
                managerId;


            leaveRequest.ApprovedAt =
                DateTime.UtcNow;




            await _leaveBalanceRepository
                .SaveChangesAsync();





            // ===============================
            // Send Approval Email
            // ===============================


            if (leaveRequest.Employee.User != null)
            {

                string emailBody =
                    $"""
                    Your Leave Request Has Been Approved

                    Leave Type:
                    {leaveRequest.LeaveType}

                    Start Date:
                    {leaveRequest.StartDate:dd-MM-yyyy}

                    End Date:
                    {leaveRequest.EndDate:dd-MM-yyyy}

                    Number Of Days:
                    {leaveRequest.NumberOfDays}

                    Approved By:
                    Manager
                    """;



                await _emailService
                    .SendEmailAsync(
                        leaveRequest.Employee.User.Email!,
                        "Leave Request Approved",
                        emailBody);
            }

        }







        public async Task RejectLeaveAsync(
            int leaveRequestId,
            int managerId)
        {

            var leaveRequest =
                await _leaveRequestRepository
                .GetByIdAsync(leaveRequestId);




            if (leaveRequest == null)
            {
                throw new NotFoundException(
                    "Leave request not found.");
            }




            if (leaveRequest.Employee.ManagerId != managerId)
            {
                throw new UnauthorizedException(
                    "You cannot reject this leave.");
            }





            if (leaveRequest.Status != LeaveStatus.Submitted)
            {
                throw new BadRequestException(
                    "Only submitted leave requests can be rejected.");
            }




            leaveRequest.Status =
                LeaveStatus.Rejected;




            await _leaveRequestRepository
                .SaveChangesAsync();






            // ===============================
            // Send Rejection Email
            // ===============================


            if (leaveRequest.Employee.User != null)
            {

                string emailBody =
                    $"""
                    Your Leave Request Has Been Rejected

                    Leave Type:
                    {leaveRequest.LeaveType}

                    Start Date:
                    {leaveRequest.StartDate:dd-MM-yyyy}

                    End Date:
                    {leaveRequest.EndDate:dd-MM-yyyy}

                    Reason:
                    {leaveRequest.Reason}

                    Please contact your manager for details.
                    """;



                await _emailService
                    .SendEmailAsync(
                        leaveRequest.Employee.User.Email!,
                        "Leave Request Rejected",
                        emailBody);
            }

        }








        public async Task CancelLeaveAsync(
            int leaveRequestId,
            int employeeId)
        {

            var leaveRequest =
                await _leaveRequestRepository
                .GetByIdAsync(leaveRequestId);



            if (leaveRequest == null)
            {
                throw new NotFoundException(
                    "Leave request not found.");
            }





            if (leaveRequest.EmployeeId != employeeId)
            {
                throw new UnauthorizedException(
                    "You cannot cancel this leave request.");
            }





            if (leaveRequest.Status != LeaveStatus.Submitted)
            {
                throw new BadRequestException(
                    "Only submitted leave requests can be cancelled.");
            }





            leaveRequest.Status =
                LeaveStatus.Cancelled;



            await _leaveRequestRepository
                .SaveChangesAsync();

        }







        private double CalculateBusinessDays(
            DateTime startDate,
            DateTime endDate)
        {

            double days = 0;



            for (
                DateTime date = startDate;
                date <= endDate;
                date = date.AddDays(1))
            {

                if (
                    date.DayOfWeek != DayOfWeek.Saturday &&
                    date.DayOfWeek != DayOfWeek.Sunday)
                {
                    days++;
                }

            }



            return days;
        }

    }
}