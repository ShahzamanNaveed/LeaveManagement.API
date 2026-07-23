namespace LeaveMangement.API.DTOs
{
    public class ManagerLeaveRequestDto
    {
        public int LeaveRequestId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string LeaveType { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double NumberOfDays { get; set; }

        public string Reason { get; set; } = string.Empty;

        public DateTime AppliedAt { get; set; }
    }
}