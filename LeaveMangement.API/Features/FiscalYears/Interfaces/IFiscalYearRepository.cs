using LeaveManagement.API.Domain.Entities;

namespace LeaveManagement.API.Features.FiscalYears.Interfaces
{
    public interface IFiscalYearRepository
    {
        Task<List<FiscalYear>>
            GetAllAsync();


        Task<FiscalYear?>
            GetByIdAsync(
                int id);


        Task<FiscalYear?>
            GetActiveAsync();


        Task<FiscalYearSetting?>
            GetFiscalYearSettingsAsync();


        Task AddAsync(
            FiscalYear fiscalYear);


        Task<bool>
            NameExistsAsync(
                string name);


        Task<bool>
            HasOverlappingDatesAsync(
                DateTime startDate,
                DateTime endDate);


        Task SaveChangesAsync();
    }
}