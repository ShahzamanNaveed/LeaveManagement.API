namespace LeaveManagement.API.Features.LeaveManagement.Dtos
{
    public class LeaveBalanceResponseDto
    {
        public string LeaveType { get; set; } = string.Empty;

        public double TotalBalance { get; set; }

        public double ConsumedBalance { get; set; }

        public double RemainingBalance { get; set; }

        public int Year { get; set; }
    }
}