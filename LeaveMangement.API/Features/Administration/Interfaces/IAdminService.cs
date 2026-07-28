using LeaveManagement.API.Features.Administration.Dtos;
using LeaveManagement.API.Features.Management.Dtos;

namespace LeaveManagement.API.Features.Administration.Interfaces
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