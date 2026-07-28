using LeaveManagement.API.Domain.Enums;

namespace LeaveManagement.API.Infrastructure.Configurations
{
    public static class LeavePolicy
    {
        public static int GetDefaultDays(
            LeaveType leaveType)
        {
            return leaveType switch
            {
                LeaveType.Annual => 14,

                LeaveType.Sick => 10,

                LeaveType.Casual => 7,

                LeaveType.Paternity => 10,

                _ => 0
            };
        }
    }
}