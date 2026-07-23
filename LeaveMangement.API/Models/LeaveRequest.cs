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




        public int? ApprovedByEmployeeId { get; set; }

        public DateTime? ApprovedAt { get; set; }



        public Employee Employee { get; set; } = null!;


        public Employee? ApprovedByEmployee { get; set; }
    }
}