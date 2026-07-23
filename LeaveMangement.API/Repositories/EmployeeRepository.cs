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
                .FirstOrDefaultAsync(e =>
                    e.Id == employeeId);
        }





        public async Task<Employee?> GetEmployeeWithManagerAsync(
            int employeeId)
        {
            return await _context.Employees

                // Employee email
                .Include(e =>
                    e.User)

                // Manager object
                .Include(e =>
                    e.Manager)

                    // Manager email
                    .ThenInclude(m =>
                        m.User)

                .FirstOrDefaultAsync(e =>
                    e.Id == employeeId);
        }





        public async Task<List<Employee>>
            GetEmployeesByManagerIdAsync(
                int managerId)
        {
            return await _context.Employees
                .Where(e =>
                    e.ManagerId == managerId)
                .ToListAsync();
        }





        public async Task<List<Employee>>
            GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e =>
                    e.Manager)
                .ToListAsync();
        }

    }
}