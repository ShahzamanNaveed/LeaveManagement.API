using LeaveManagement.API.Domain.Entities;

namespace LeaveManagement.API.Features.FiscalYears.Interfaces
{
    public interface IFiscalYearSettingsRepository
    {
        Task<FiscalYearSetting?>
            GetAsync();

        Task AddAsync(
            FiscalYearSetting settings);

        Task SaveChangesAsync();
    }
}