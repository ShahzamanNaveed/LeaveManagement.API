namespace LeaveMangement.API.DTOs
{
    public class FiscalYearSettingsResponseDto
    {
        public int Id { get; set; }

        public int StartMonth { get; set; }

        public int StartDay { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}