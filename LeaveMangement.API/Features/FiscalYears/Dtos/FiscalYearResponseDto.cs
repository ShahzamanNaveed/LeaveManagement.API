namespace LeaveManagement.API.Features.FiscalYears.Dtos
{
    public class FiscalYearResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
            = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}