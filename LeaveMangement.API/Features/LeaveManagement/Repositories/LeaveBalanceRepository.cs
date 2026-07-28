using LeaveManagement.API.Domain.Entities;
using LeaveManagement.API.Domain.Enums;
using LeaveManagement.API.Features.LeaveManagement.Interfaces;
using LeaveManagement.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.API.Features.LeaveManagement.Repositories
{
    public class LeaveBalanceRepository : ILeaveBalanceRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveBalanceRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(
            List<LeaveBalance> leaveBalances)
        {
            await _context.LeaveBalances
                .AddRangeAsync(leaveBalances);
        }

        public async Task<LeaveBalance?> GetBalanceAsync(
            int employeeId,
            LeaveType leaveType,
            int fiscalYearId)
        {
            return await _context.LeaveBalances
                .FirstOrDefaultAsync(l =>
                    l.EmployeeId == employeeId &&
                    l.LeaveType == leaveType &&
                    l.FiscalYearId == fiscalYearId);
        }

        public async Task<List<LeaveBalance>> GetEmployeeBalancesAsync(
            int employeeId)
        {
            return await _context.LeaveBalances
                .Where(l => l.EmployeeId == employeeId)
                .OrderBy(l => l.LeaveType)
                .ToListAsync();
        }

        // =====================================
        // Check if balances already exist
        // =====================================

        public async Task<bool> ExistsAsync(
     int employeeId,
     int fiscalYearId)
        {
            return await _context.LeaveBalances
                .AnyAsync(x =>
                    x.EmployeeId == employeeId &&
                    x.FiscalYearId == fiscalYearId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}