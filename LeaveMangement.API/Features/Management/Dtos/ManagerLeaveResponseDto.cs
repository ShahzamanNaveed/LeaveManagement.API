namespace LeaveManagement.API.Features.Management.Dtos
{
    public class ManagerLeaveResponseDto
    {
        public int Id { get; set; }


        public string EmployeeName { get; set; } = string.Empty;


        public string LeaveType { get; set; } = string.Empty;


        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }


        public double NumberOfDays { get; set; }


        public bool IsHalfDay { get; set; }


        public string Reason { get; set; } = string.Empty;


        public string Status { get; set; } = string.Empty;


        public DateTime AppliedAt { get; set; }
    }
}