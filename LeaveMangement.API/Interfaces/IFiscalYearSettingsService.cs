using LeaveMangement.API.DTOs;

namespace LeaveMangement.API.Interfaces
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