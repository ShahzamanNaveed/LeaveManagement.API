using LeaveManagement.API.Domain.Entities;
using LeaveManagement.API.Domain.Enums;

namespace LeaveManagement.API.Features.LeaveManagement.Interfaces
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