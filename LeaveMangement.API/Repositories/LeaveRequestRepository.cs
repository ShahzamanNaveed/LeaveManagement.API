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

                .Include(l =>
                    l.Employee)

                    .ThenInclude(e =>
                        e.User)

                .Include(l =>
                    l.Approvals)

                .Where(l =>

                    l.Status == LeaveStatus.Submitted &&

                    l.Approvals.Any(a =>
                        a.ManagerId == managerId &&
                        a.Status == LeaveStatus.Submitted))

                .OrderByDescending(l =>
                    l.AppliedAt)

                .ToListAsync();
        }





        public async Task<LeaveRequest?> GetByIdAsync(
            int id)
        {
            return await _context.LeaveRequests

                .Include(l =>
                    l.Employee)

                    .ThenInclude(e =>
                        e.User)

                .Include(l =>
                    l.Approvals)

                        .ThenInclude(a =>
                            a.Manager)

                                .ThenInclude(m =>
                                    m.User)

                .FirstOrDefaultAsync(l =>
                    l.Id == id);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }



        public async Task AddLeaveApprovalsAsync(
    List<LeaveApproval> approvals)
        {
            await _context.LeaveApprovals
                .AddRangeAsync(approvals);
        }





        public async Task<LeaveApproval?>
            GetManagerApprovalAsync(
                int leaveRequestId,
                int managerId)
        {
            return await _context.LeaveApprovals
                .FirstOrDefaultAsync(a =>
                    a.LeaveRequestId == leaveRequestId &&
                    a.ManagerId == managerId);
        }





        public async Task<List<LeaveApproval>>
            GetApprovalsAsync(
                int leaveRequestId)
        {
            return await _context.LeaveApprovals
                .Where(a =>
                    a.LeaveRequestId == leaveRequestId)
                .ToListAsync();
        }
    }
}