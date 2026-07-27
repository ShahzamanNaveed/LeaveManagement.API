using LeaveManagement.API.Models;

namespace LeaveManagement.API.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public bool IsManager { get; set; } = false;



        // =========================
        // Navigation Properties
        // =========================

        public ApplicationUser User { get; set; } = null!;



        // =========================
        // Multi Manager Navigation
        // =========================

        // Managers assigned to this employee
        public ICollection<EmployeeManagerAssignment>
            ManagerAssignments
        { get; set; }
            = new List<EmployeeManagerAssignment>();



        // Employees managed by this manager
        public ICollection<EmployeeManagerAssignment>
            EmployeeAssignments
        { get; set; }
            = new List<EmployeeManagerAssignment>();
    }
}