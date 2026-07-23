using LeaveMangement.API.DTOs;

namespace LeaveMangement.API.Interfaces
{
    public interface IAdminService
    {

        Task<List<AdminEmployeeResponseDto>>
            GetAllEmployeesAsync();


        Task CreateEmployeeAsync(
            CreateEmployeeRequestDto request);


        Task CreateManagerAsync(
            CreateManagerRequestDto request);


        Task AssignManagerAsync(
            int employeeId,
            AssignManagerRequestDto request);

    }
}