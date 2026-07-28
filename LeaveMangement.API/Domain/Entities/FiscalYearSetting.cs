namespace LeaveManagement.API.Domain.Entities
{
    public class FiscalYearSetting
    {
        public int Id { get; set; }


        // =========================
        // Fiscal Year Start Rule
        // =========================

        public int StartMonth { get; set; }

        public int StartDay { get; set; }



        // =========================
        // Metadata
        // =========================

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;
    }
}