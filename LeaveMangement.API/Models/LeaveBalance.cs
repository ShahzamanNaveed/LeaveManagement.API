using LeaveManagement.API.Enums;

namespace LeaveManagement.API.Models
{
    public class LeaveBalance
    {
        public int Id { get; set; }


        public int EmployeeId { get; set; }


        public LeaveType LeaveType { get; set; }


        public double TotalBalance { get; set; }


        public double ConsumedBalance { get; set; }


        public double RemainingBalance { get; set; }



        // =========================
        // Fiscal Year
        // =========================

        public int FiscalYearId { get; set; }


        public FiscalYear FiscalYear { get; set; }
            = null!;



        // Navigation

        public Employee Employee { get; set; }
            = null!;
    }
}