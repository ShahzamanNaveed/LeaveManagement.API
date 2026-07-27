using LeaveMangement.API.DTOs;

namespace LeaveMangement.API.Interfaces
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