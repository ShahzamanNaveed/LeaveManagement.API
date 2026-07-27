using LeaveManagement.API.Models;

namespace LeaveMangement.API.Interfaces
{
    public interface IFiscalYearService
    {
        Task<FiscalYear> GetActiveFiscalYearAsync();
    }
}