namespace LeaveManagement.API.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public int? ManagerId { get; set; }

        public bool IsManager { get; set; } = false;



        // Navigation Properties

        public ApplicationUser User { get; set; } = null!;

        public Employee? Manager { get; set; }

        public ICollection<Employee> Employees { get; set; }
            = new List<Employee>();
    }
}