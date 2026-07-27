namespace LeaveManagement.API.Models
{
    public class EmployeeManagerAssignment
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int ManagerId { get; set; }

        public DateTime AssignedOn { get; set; }
            = DateTime.UtcNow;

        public bool IsActive { get; set; }
            = true;



        // =========================
        // Navigation Properties
        // =========================

        public Employee Employee { get; set; } = null!;

        public Employee Manager { get; set; } = null!;
    }
}