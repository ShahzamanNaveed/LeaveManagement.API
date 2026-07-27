using LeaveManagement.API.Enums;

namespace LeaveManagement.API.Models
{
    public class LeaveApproval
    {
        public int Id { get; set; }

        public int LeaveRequestId { get; set; }

        public int ManagerId { get; set; }

        public LeaveStatus Status { get; set; }
            = LeaveStatus.Submitted;

        public DateTime? ActionAt { get; set; }

        // Optional
        public string? Remarks { get; set; }



        // =========================
        // Navigation Properties
        // =========================

        public LeaveRequest LeaveRequest { get; set; }
            = null!;

        public Employee Manager { get; set; }
            = null!;
    }
}