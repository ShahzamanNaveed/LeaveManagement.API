using FluentValidation;
using LeaveManagement.API.Features.Authentication.Dtos;

namespace LeaveManagement.API.Common.Validators.Authentication
{
    public class RegisterRequestValidator
        : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .MaximumLength(100)
                .WithMessage("Full name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters.")
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d")
                .WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]")
                .WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.Department)
                .NotEmpty()
                .WithMessage("Department is required.")
                .MaximumLength(100)
                .WithMessage("Department cannot exceed 100 characters.");

            RuleFor(x => x.Designation)
                .NotEmpty()
                .WithMessage("Designation is required.")
                .MaximumLength(100)
                .WithMessage("Designation cannot exceed 100 characters.");
        }
    }
}