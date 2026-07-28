using LeaveManagement.API.Features.Authentication.Dtos;

namespace LeaveManagement.API.Features.Authentication.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto request);

        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}