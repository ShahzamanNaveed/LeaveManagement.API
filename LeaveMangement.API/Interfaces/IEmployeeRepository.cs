using LeaveManagement.API.Models;

namespace LeaveMangement.API.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);


        Task SaveChangesAsync();


        Task<Employee?> GetByUserIdAsync(
            string userId);



        Task<Employee?> GetByIdAsync(
            int employeeId);



        Task<Employee?> GetEmployeeWithManagerAsync(
            int employeeId);



        Task<List<Employee>> GetEmployeesByManagerIdAsync(
            int managerId);



        Task<List<Employee>> GetAllEmployeesAsync();
    }
}