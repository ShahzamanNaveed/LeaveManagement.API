using LeaveManagement.API.Domain.Enums;
using LeaveManagement.API.Features.LeaveManagement.Interfaces;
using LeaveManagement.API.Features.Management.Dtos;
using LeaveManagement.API.Features.Management.Interfaces;

namespace LeaveManagement.API.Features.Management.Services
{
    public class ManagerService : IManagerService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public ManagerService(
            ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<List<ManagerLeaveResponseDto>>
            GetPendingLeavesAsync(int managerId)
        {
            var requests =
                await _leaveRequestRepository
                .GetManagerRequestsAsync(
                    managerId,
                    LeaveStatus.Submitted);

            return requests.Select(l => new ManagerLeaveResponseDto
            {
                Id = l.Id,

                EmployeeName = l.Employee.FullName,

                LeaveType = l.LeaveType.ToString(),

                StartDate = l.StartDate,

                EndDate = l.EndDate,

                NumberOfDays = l.NumberOfDays,

                IsHalfDay = l.IsHalfDay,

                Reason = l.Reason,

                Status = l.Status.ToString(),

                AppliedAt = l.AppliedAt

            }).ToList();
        }
    }
}