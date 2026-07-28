using LeaveManagement.API.Features.Management.Dtos;

namespace LeaveManagement.API.Features.Management.Interfaces
{
    public interface IManagerService
    {
        Task<List<ManagerLeaveResponseDto>>
            GetPendingLeavesAsync(int managerId);
    }
}