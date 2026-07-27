using LeaveManagement.API.Enums;

namespace LeaveManagement.API.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public LeaveType LeaveType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double NumberOfDays { get; set; }

        public bool IsHalfDay { get; set; }

        public string Reason { get; set; } = string.Empty;

        public LeaveStatus Status { get; set; }
            = LeaveStatus.Submitted;

        public DateTime AppliedAt { get; set; }
            = DateTime.UtcNow;



        // =========================
        // Navigation Properties
        // =========================

        public Employee Employee { get; set; } = null!;



        // =========================
        // Manager Approvals
        // =========================

        public ICollection<LeaveApproval>
            Approvals
        { get; set; }
            = new List<LeaveApproval>();

    }
}