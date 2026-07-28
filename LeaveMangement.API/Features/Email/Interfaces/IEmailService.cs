namespace LeaveManagement.API.Features.Email.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(
            string toEmail,
            string subject,
            string body);
    }
}