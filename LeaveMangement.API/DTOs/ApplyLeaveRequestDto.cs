using LeaveManagement.API.Enums;

namespace LeaveMangement.API.DTOs
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