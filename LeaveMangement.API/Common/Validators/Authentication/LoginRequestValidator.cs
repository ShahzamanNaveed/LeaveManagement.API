using FluentValidation;
using LeaveManagement.API.Features.Authentication.Dtos;

namespace LeaveManagement.API.Common.Validators.Authentication
{
    public class LoginRequestValidator
        : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}