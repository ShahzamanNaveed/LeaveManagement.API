using LeaveManagement.API.Models;

namespace LeaveMangement.API.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task AddAsync(LeaveRequest leaveRequest);

        Task<bool> HasPendingRequestAsync(
            int employeeId);

        Task<bool> HasOverlappingRequestAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate);

        Task<List<LeaveRequest>> GetEmployeeLeavesAsync(
            int employeeId);

        Task<List<LeaveRequest>> GetPendingRequestsForManagerAsync(
            int managerId);

        Task<LeaveRequest?> GetByIdAsync(
            int id);

        Task SaveChangesAsync();
    }
}