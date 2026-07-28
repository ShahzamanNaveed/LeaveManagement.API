using LeaveManagement.API.Domain.Enums;

namespace LeaveManagement.API.Features.LeaveManagement.Dtos
{
    public class ApplyLeaveRequestDto
    {
        public LeaveType LeaveType { get; set; }


        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }


        public bool IsHalfDay { get; set; }


        public string Reason { get; set; } = string.Empty;
    }
}