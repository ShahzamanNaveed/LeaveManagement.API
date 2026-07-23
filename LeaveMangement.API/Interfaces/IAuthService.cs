using LeaveMangement.API.DTOs;

namespace LeaveMangement.API.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto request);

        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}