using LeaveManagement.API.Data;
using LeaveManagement.API.Enums;
using LeaveManagement.API.Models;
using LeaveMangement.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.API.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _context;


        public LeaveRequestRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task AddAsync(
            LeaveRequest leaveRequest)
        {
            await _context.LeaveRequests
                .AddAsync(leaveRequest);
        }



        public async Task<bool> HasPendingRequestAsync(
            int employeeId)
        {
            return await _context.LeaveRequests
                .AnyAsync(l =>
                    l.EmployeeId == employeeId &&
                    l.Status == LeaveStatus.Submitted);
        }



        public async Task<bool> HasOverlappingRequestAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _context.LeaveRequests
                .AnyAsync(l =>
                    l.EmployeeId == employeeId &&
                    l.Status != LeaveStatus.Rejected &&
                    startDate <= l.EndDate &&
                    endDate >= l.StartDate);
        }




        public async Task<List<LeaveRequest>>
            GetEmployeeLeavesAsync(
                int employeeId)
        {
            return await _context.LeaveRequests
                .Where(l =>
                    l.EmployeeId == employeeId)
                .OrderByDescending(l =>
                    l.AppliedAt)
                .ToListAsync();
        }





        public async Task<List<LeaveRequest>>
            GetPendingRequestsForManagerAsync(
                int managerId)
        {
            return await _context.LeaveRequests

                // Load employee who applied leave
                .Include(l =>
                    l.Employee)

                // Load employee's Identity user
                // for email access
                .ThenInclude(e =>
                    e.User)

                .Where(l =>
                    l.Employee.ManagerId == managerId &&
                    l.Status == LeaveStatus.Submitted)

                .OrderByDescending(l =>
                    l.AppliedAt)

                .ToListAsync();
        }





        public async Task<LeaveRequest?> GetByIdAsync(
            int id)
        {
            return await _context.LeaveRequests

                // Load employee
                .Include(l =>
                    l.Employee)

                // Load employee email
                .ThenInclude(e =>
                    e.User)

                .FirstOrDefaultAsync(l =>
                    l.Id == id);
        }





        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}