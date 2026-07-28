namespace LeaveManagement.API.Features.Authentication.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(
           string userId,
            int employeeId,
            string email,
            string role);
    }
}