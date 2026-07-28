using LeaveManagement.API.Domain.Entities;

namespace LeaveManagement.API.Features.FiscalYears.Interfaces
{
    public interface IFiscalYearService
    {
        Task<FiscalYear> GetActiveFiscalYearAsync();
    }
}