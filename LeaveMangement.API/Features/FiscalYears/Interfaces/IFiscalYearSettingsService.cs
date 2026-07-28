using LeaveManagement.API.Features.FiscalYears.Dtos;

namespace LeaveManagement.API.Features.FiscalYears.Interfaces
{
    public interface IFiscalYearSettingsService
    {
        Task<FiscalYearSettingsResponseDto>
            GetAsync();

        Task<FiscalYearSettingsResponseDto>
            CreateAsync(
                CreateFiscalYearSettingsDto request);

        Task<FiscalYearSettingsResponseDto>
            UpdateAsync(
                CreateFiscalYearSettingsDto request);
    }
}