using LeaveManagement.API.Models;

namespace LeaveMangement.API.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddAsync(
            Employee employee);

        Task SaveChangesAsync();

        Task<Employee?> GetByUserIdAsync(
            string userId);

        Task<Employee?> GetByIdAsync(
            int employeeId);

        Task<List<Employee>>
            GetAllEmployeesAsync();



        // =========================
        // Multi Manager
        // =========================

        Task<List<EmployeeManagerAssignment>>
            GetManagerAssignmentsAsync(
                int employeeId);

        Task<List<Employee>>
            GetManagersByEmployeeIdAsync(
                int employeeId);

        Task<List<Employee>>
            GetEmployeesByIdsAsync(
                List<int> employeeIds);

        Task AddManagerAssignmentsAsync(
            List<EmployeeManagerAssignment> assignments);

        void RemoveManagerAssignments(
            List<EmployeeManagerAssignment> assignments);
    }
}