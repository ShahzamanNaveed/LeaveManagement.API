namespace LeaveManagement.API.Features.FiscalYears.Dtos
{
    public class CreateFiscalYearDto
    {
        public string Name { get; set; }
            = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}