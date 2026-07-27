using LeaveManagement.API.Models;

namespace LeaveMangement.API.Interfaces
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