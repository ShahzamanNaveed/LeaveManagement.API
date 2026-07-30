using LeaveManagement.API.Common.Exceptions;
using LeaveManagement.API.Features.Email.Interfaces;
using LeaveManagement.API.Features.Employees.Interfaces;
using LeaveManagement.API.Features.FiscalYears.Interfaces;
using LeaveManagement.API.Features.LeaveManagement.Dtos;
using LeaveManagement.API.Features.LeaveManagement.Interfaces;
using LeaveManagement.API.Features.Management.Dtos;
using LeaveManagement.API.Domain.Entities;
using LeaveManagement.API.Domain.Enums;
using LeaveManagement.API.Features.Email.Templates;

namespace LeaveManagement.API.Features.LeaveManagement.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        private readonly ILeaveBalanceRepository _leaveBalanceRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IEmailService _emailService;

        private readonly IFiscalYearService _fiscalYearService;

        public LeaveService(
     ILeaveRequestRepository leaveRequestRepository,
     ILeaveBalanceRepository leaveBalanceRepository,
     IEmployeeRepository employeeRepository,
     IEmailService emailService,
     IFiscalYearService fiscalYearService)
        {
            _leaveRequestRepository =
                leaveRequestRepository;

            _leaveBalanceRepository =
                leaveBalanceRepository;

            _employeeRepository =
                employeeRepository;

            _emailService =
                emailService;

            _fiscalYearService =
                fiscalYearService;
        }


        // =====================================================
        // APPLY LEAVE
        // =====================================================

        public async Task ApplyLeaveAsync(
            int employeeId,
            ApplyLeaveRequestDto request)
        {



            // =====================================================
            // Validate Past Date
            // =====================================================

            if (request.StartDate.Date < DateTime.UtcNow.Date)
            {
                throw new BadRequestException(
                    "Leave cannot be applied for past dates.");
            }

            // =====================================================
            // Validate Half-Day Leave
            // =====================================================

            if (request.IsHalfDay)
            {
                if (request.StartDate.Date != request.EndDate.Date)
                {
                    throw new BadRequestException(
                        "Half-day leave can only be applied for a single day.");
                }

                if (request.StartDate.DayOfWeek == DayOfWeek.Saturday ||
                    request.StartDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    throw new BadRequestException(
                        "Half-day leave cannot be applied on weekends.");
                }
            }

            // =====================================================
            // Validate Weekend Leave
            // =====================================================

            if (!request.IsHalfDay)
            {
                double businessDays =
                    CalculateBusinessDays(
                        request.StartDate,
                        request.EndDate);

                if (businessDays == 0)
                {
                    throw new BadRequestException(
                        "Leave cannot be applied only for weekends.");
                }
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



            var fiscalYear =
    await _fiscalYearService
    .GetActiveFiscalYearAsync();



            var balance =
                await _leaveBalanceRepository
                .GetBalanceAsync(
                    employeeId,
                    request.LeaveType,
                    fiscalYear.Id);





            if (balance == null)
            {
                throw new NotFoundException(
                    "Leave balance not found.");
            }


            var pendingLeaveDays =
    await _leaveRequestRepository
    .GetPendingLeaveDaysAsync(
        employeeId,
        request.LeaveType);

            var availableBalance =
                balance.RemainingBalance - pendingLeaveDays;

            if (availableBalance < numberOfDays)
            {
                throw new BadRequestException(
                    $"Insufficient leave balance. Available: {availableBalance} day(s).");
            }


            // =====================================================
            // Get Assigned Managers
            // =====================================================

            var managers =
                await _employeeRepository
                .GetManagersByEmployeeIdAsync(employeeId);



            if (!managers.Any())
            {
                throw new BadRequestException(
                    "No manager assigned to employee.");
            }


            // =====================================================
            // Create Leave Request
            // =====================================================

            var leaveRequest =
                new LeaveRequest
                {
                    EmployeeId = employeeId,

                    LeaveType =
                        request.LeaveType,

                    StartDate =
                        request.StartDate,

                    EndDate =
                        request.EndDate,

                    NumberOfDays =
                        numberOfDays,

                    IsHalfDay =
                        request.IsHalfDay,

                    Reason =
                        request.Reason,

                    Status =
                        LeaveStatus.Submitted,

                    AppliedAt =
                        DateTime.UtcNow
                };


            await _leaveRequestRepository
                .AddAsync(leaveRequest);


            await _leaveRequestRepository
                .SaveChangesAsync();


            // =====================================================
            // Create Manager Approval Records
            // =====================================================

            var approvals =
                managers
                .Select(manager =>
                    new LeaveApproval
                    {
                        LeaveRequestId =
                            leaveRequest.Id,

                        ManagerId =
                            manager.Id,

                        Status =
                            LeaveStatus.Submitted
                    })
                .ToList();



            await _leaveRequestRepository
                .AddLeaveApprovalsAsync(
                    approvals);


            await _leaveRequestRepository
                .SaveChangesAsync();


            // =====================================================
            // Send Email To Managers
            // =====================================================

            var employee =
                await _employeeRepository
                .GetByIdAsync(employeeId);


            foreach (var manager in managers)
            {

                if (manager.User == null)
                    continue;

                string emailBody =
    LeaveSubmittedTemplate.Build(
        employee!.FullName,
        employee.Department,
        request.LeaveType.ToString(),
        request.StartDate,
        request.EndDate,
        numberOfDays,
        request.Reason);


                await _emailService
                    .SendEmailAsync(
                        manager.User.Email!,
                        "New Leave Request Submitted",
                        emailBody);
            }
        }

        // =====================================================
        // GET MY LEAVES
        // =====================================================

        public async Task<List<LeaveResponseDto>>
            GetMyLeavesAsync(
                int employeeId)
        {

            var leaves =
                await _leaveRequestRepository
                .GetEmployeeLeavesAsync(employeeId);

            return leaves
                .Select(l =>
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

                    })
                .ToList();
        }


        // =====================================================
        // GET MY LEAVE BALANCES
        // =====================================================

        public async Task<List<LeaveBalanceResponseDto>>
            GetMyBalancesAsync(
                int employeeId)
        {

            var balances =
                await _leaveBalanceRepository
                .GetEmployeeBalancesAsync(employeeId);



            return balances
 .Select(b =>
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
             b.FiscalYear.StartDate.Year

     })
 .ToList();
        }

        // =====================================================
        // GET PENDING REQUESTS FOR MANAGER
        // =====================================================

        public async Task<List<ManagerLeaveResponseDto>>
    GetManagerRequestsAsync(
        int managerId,
        LeaveStatus? status)
        {
            var requests =
                await _leaveRequestRepository
                .GetManagerRequestsAsync(
                    managerId,
                    status);

            return requests
                .Select(l =>
                    new ManagerLeaveResponseDto
                    {
                        Id =
                            l.Id,

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
                    })
                .ToList();
        }


        // =====================================================
        // APPROVE LEAVE
        // =====================================================

        public async Task ApproveLeaveAsync(
            int leaveRequestId,
            int managerId)
        {

            var leaveRequest =
                await _leaveRequestRepository
                .GetByIdAsync(
                    leaveRequestId);


            if (leaveRequest == null)
            {
                throw new NotFoundException(
                    "Leave request not found.");
            }



            if (leaveRequest.Status != LeaveStatus.Submitted)
            {
                throw new BadRequestException(
                    "This leave request is already processed.");
            }


            // =====================================================
            // Find Manager Approval Record
            // =====================================================

            var approval =
                await _leaveRequestRepository
                .GetManagerApprovalAsync(
                    leaveRequestId,
                    managerId);


            if (approval == null)
            {
                throw new UnauthorizedException(
                    "You are not assigned to approve this leave.");
            }



            if (approval.Status != LeaveStatus.Submitted)
            {
                throw new BadRequestException(
                    "You have already responded to this request.");
            }


            // =====================================================
            // Approve Current Manager
            // =====================================================

            approval.Status =
                LeaveStatus.Approved;


            approval.ActionAt =
                DateTime.UtcNow;



            await _leaveRequestRepository
                .SaveChangesAsync();


            // =====================================================
            // Check All Managers Approved
            // =====================================================

            var approvals =
                await _leaveRequestRepository
                .GetApprovalsAsync(
                    leaveRequestId);



            bool allApproved =
                approvals.All(a =>
                    a.Status == LeaveStatus.Approved);


            if (!allApproved)
            {
                return;
            }

            // =====================================================
            // Final Leave Approval Processing
            // =====================================================

            var fiscalYear =
    await _fiscalYearService
    .GetActiveFiscalYearAsync();

            var balance =
                await _leaveBalanceRepository
                .GetBalanceAsync(
                    leaveRequest.EmployeeId,
                    leaveRequest.LeaveType,
                    fiscalYear.Id);



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


            await _leaveBalanceRepository
                .SaveChangesAsync();


            // =====================================================
            // Notify Employee
            // =====================================================

            if (leaveRequest.Employee.User != null)
            {

                string emailBody =
     LeaveApprovedTemplate.Build(
         leaveRequest.LeaveType.ToString(),
         leaveRequest.StartDate,
         leaveRequest.EndDate,
         leaveRequest.NumberOfDays);



                await _emailService
                    .SendEmailAsync(
                        leaveRequest.Employee.User.Email!,
                        "Leave Request Approved",
                        emailBody);
            }

        }

        // =====================================================
        // REJECT LEAVE
        // =====================================================

        public async Task RejectLeaveAsync(
            int leaveRequestId,
            int managerId)
        {

            var leaveRequest =
                await _leaveRequestRepository
                .GetByIdAsync(
                    leaveRequestId);



            if (leaveRequest == null)
            {
                throw new NotFoundException(
                    "Leave request not found.");
            }


            if (leaveRequest.Status != LeaveStatus.Submitted)
            {
                throw new BadRequestException(
                    "This leave request is already processed.");
            }





            // =====================================================
            // Find Manager Approval Record
            // =====================================================

            var approval =
                await _leaveRequestRepository
                .GetManagerApprovalAsync(
                    leaveRequestId,
                    managerId);


            if (approval == null)
            {
                throw new UnauthorizedException(
                    "You are not assigned to approve this leave.");
            }


            if (approval.Status != LeaveStatus.Submitted)
            {
                throw new BadRequestException(
                    "You have already responded to this request.");
            }


            // =====================================================
            // Reject Manager Approval
            // =====================================================

            approval.Status =
                LeaveStatus.Rejected;


            approval.ActionAt =
                DateTime.UtcNow;


            // =====================================================
            // Reject Complete Leave Request
            // =====================================================

            leaveRequest.Status =
                LeaveStatus.Rejected;


            await _leaveRequestRepository
                .SaveChangesAsync();


            // =====================================================
            // Notify Employee
            // =====================================================

            if (leaveRequest.Employee.User != null)
            {

                string emailBody =
    LeaveRejectedTemplate.Build(
        leaveRequest.LeaveType.ToString(),
        leaveRequest.StartDate,
        leaveRequest.EndDate,
        leaveRequest.NumberOfDays);


                await _emailService
                    .SendEmailAsync(
                        leaveRequest.Employee.User.Email!,
                        "Leave Request Rejected",
                        emailBody);
            }

        }

        // =====================================================
        // CANCEL LEAVE
        // =====================================================

        public async Task CancelLeaveAsync(
            int leaveRequestId,
            int employeeId)
        {

            var leaveRequest =
                await _leaveRequestRepository
                .GetByIdAsync(
                    leaveRequestId);



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


            // =====================================================
            // Cancel Pending Manager Approvals
            // =====================================================

            var approvals =
                await _leaveRequestRepository
                .GetApprovalsAsync(
                    leaveRequestId);


            foreach (var approval in approvals)
            {

                if (approval.Status ==
                    LeaveStatus.Submitted)
                {

                    approval.Status =
                        LeaveStatus.Cancelled;


                    approval.ActionAt =
                        DateTime.UtcNow;
                }
            }



            await _leaveRequestRepository
                .SaveChangesAsync();


            // =====================================================
            // Notify Managers
            // =====================================================

            foreach (var approval in approvals)
            {

                if (approval.Manager?.User == null)
                    continue;


                string emailBody =
    LeaveCancelledTemplate.Build(
        leaveRequest.Employee.FullName,
        leaveRequest.LeaveType.ToString(),
        leaveRequest.StartDate,
        leaveRequest.EndDate);


                await _emailService
                    .SendEmailAsync(
                        approval.Manager.User.Email!,
                        "Leave Request Cancelled",
                        emailBody);

            }

        }

        // =====================================================
        // CALCULATE BUSINESS DAYS
        // =====================================================

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