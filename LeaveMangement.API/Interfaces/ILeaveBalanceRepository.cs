using LeaveManagement.API.Enums;
using LeaveManagement.API.Models;

namespace LeaveMangement.API.Interfaces
{
    public interface ILeaveBalanceRepository
    {
        Task AddRangeAsync(
            List<LeaveBalance> leaveBalances);

        Task<LeaveBalance?> GetBalanceAsync(
            int employeeId,
            LeaveType leaveType,
            int fiscalYearId);

        Task<List<LeaveBalance>> GetEmployeeBalancesAsync(
            int employeeId);

        Task<bool> ExistsAsync(
            int employeeId,
            int fiscalYearId);

        Task SaveChangesAsync();
    }
}