using LeaveManagement.API.Data;
using LeaveManagement.API.Models;
using LeaveMangement.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            Employee employee)
        {
            await _context.Employees
                .AddAsync(employee);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Employee?> GetByUserIdAsync(
            string userId)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e =>
                    e.UserId == userId);
        }

        public async Task<Employee?> GetByIdAsync(
            int employeeId)
        {
            return await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e =>
                    e.Id == employeeId);
        }

        public async Task<List<Employee>>
            GetAllEmployeesAsync()
        {
            return await _context.Employees

                .Include(e =>
                    e.ManagerAssignments)

                    .ThenInclude(a =>
                        a.Manager)

                .ToListAsync();
        }

        // =========================
        // Multi Manager
        // =========================

        public async Task<List<EmployeeManagerAssignment>>
            GetManagerAssignmentsAsync(
                int employeeId)
        {
            return await _context.EmployeeManagerAssignments

                .Where(a =>
                    a.EmployeeId == employeeId &&
                    a.IsActive)

                .ToListAsync();
        }

        public async Task<List<Employee>>
            GetManagersByEmployeeIdAsync(
                int employeeId)
        {
            return await _context.EmployeeManagerAssignments

                .Where(a =>
                    a.EmployeeId == employeeId &&
                    a.IsActive)

                .Include(a =>
                    a.Manager)

                        .ThenInclude(m =>
                            m.User)

                .Select(a =>
                    a.Manager)

                .ToListAsync();
        }

        public async Task<List<Employee>>
            GetEmployeesByIdsAsync(
                List<int> employeeIds)
        {
            return await _context.Employees

                .Where(e =>
                    employeeIds.Contains(e.Id))

                .ToListAsync();
        }

        public async Task AddManagerAssignmentsAsync(
            List<EmployeeManagerAssignment> assignments)
        {
            await _context.EmployeeManagerAssignments
                .AddRangeAsync(assignments);
        }

        public void RemoveManagerAssignments(
            List<EmployeeManagerAssignment> assignments)
        {
            _context.EmployeeManagerAssignments
                .RemoveRange(assignments);
        }
    }
}