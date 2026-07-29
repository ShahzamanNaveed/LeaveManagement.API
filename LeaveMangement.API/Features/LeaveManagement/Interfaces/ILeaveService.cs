using LeaveManagement.API.Domain.Enums;
using LeaveManagement.API.Features.LeaveManagement.Dtos;
using LeaveManagement.API.Features.Management.Dtos;

namespace LeaveManagement.API.Features.LeaveManagement.Interfaces
{
    public interface ILeaveService
    {
        Task ApplyLeaveAsync(
            int employeeId,
            ApplyLeaveRequestDto request);

        Task<List<LeaveResponseDto>> GetMyLeavesAsync(
            int employeeId);

        Task<List<LeaveBalanceResponseDto>> GetMyBalancesAsync(
            int employeeId);

        Task<List<ManagerLeaveResponseDto>> GetManagerRequestsAsync(
            int managerId,
            LeaveStatus? status);

        Task ApproveLeaveAsync(
            int leaveRequestId,
            int managerId);

        Task RejectLeaveAsync(
            int leaveRequestId,
            int managerId);

        Task CancelLeaveAsync(
            int leaveRequestId,
            int employeeId);
    }
}