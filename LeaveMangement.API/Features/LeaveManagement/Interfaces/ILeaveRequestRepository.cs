using LeaveManagement.API.Domain.Entities;
using LeaveManagement.API.Domain.Enums;

namespace LeaveManagement.API.Features.LeaveManagement.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task AddAsync(
            LeaveRequest leaveRequest);

        Task AddLeaveApprovalsAsync(
            List<LeaveApproval> approvals);

        Task<double> GetPendingLeaveDaysAsync(
            int employeeId,
            LeaveType leaveType);

        Task<bool> HasOverlappingRequestAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate);

        Task<List<LeaveRequest>>
            GetEmployeeLeavesAsync(
                int employeeId);

        Task<List<LeaveRequest>>
            GetManagerRequestsAsync(
                int managerId,
                LeaveStatus? status);

        Task<LeaveRequest?> GetByIdAsync(
            int id);

        Task<LeaveApproval?> GetManagerApprovalAsync(
            int leaveRequestId,
            int managerId);

        Task<List<LeaveApproval>>
            GetApprovalsAsync(
                int leaveRequestId);

        Task SaveChangesAsync();
    }
}