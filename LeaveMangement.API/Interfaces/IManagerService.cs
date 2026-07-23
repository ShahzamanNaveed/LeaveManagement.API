using LeaveMangement.API.DTOs;

namespace LeaveMangement.API.Interfaces
{
    public interface IManagerService
    {
        Task<List<ManagerLeaveResponseDto>>
            GetPendingLeavesAsync(int managerId);
    }
}