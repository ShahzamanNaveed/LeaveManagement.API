using LeaveManagement.API.Features.FiscalYears.Dtos;

namespace LeaveManagement.API.Features.FiscalYears.Interfaces
{
    public interface IFiscalYearManagementService
    {
        Task<List<FiscalYearResponseDto>>
            GetAllAsync();


        Task<FiscalYearResponseDto>
            CreateAsync(
                CreateFiscalYearDto request);


        Task ActivateAsync(
            int fiscalYearId);


        Task<FiscalYearResponseDto>
            GenerateNextAsync();
    }
}