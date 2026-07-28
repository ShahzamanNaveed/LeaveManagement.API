using LeaveManagement.API.Domain.Entities;

namespace LeaveManagement.API.Features.LeaveManagement.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task AddAsync(
            LeaveRequest leaveRequest);

        Task AddLeaveApprovalsAsync(
            List<LeaveApproval> approvals);

        Task<bool> HasPendingRequestAsync(
            int employeeId);

        Task<bool> HasOverlappingRequestAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate);

        Task<List<LeaveRequest>>
            GetEmployeeLeavesAsync(
                int employeeId);

        Task<List<LeaveRequest>>
            GetPendingRequestsForManagerAsync(
                int managerId);

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