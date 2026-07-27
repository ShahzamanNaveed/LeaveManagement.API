namespace LeaveManagement.API.Models
{
    public class FiscalYear
    {
        public int Id { get; set; }


        // =========================
        // Fiscal Year Information
        // =========================

        public string Name { get; set; }
            = string.Empty;


        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }



        // =========================
        // Status
        // =========================

        public bool IsActive { get; set; }



        // =========================
        // Metadata
        // =========================

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;
    }
}