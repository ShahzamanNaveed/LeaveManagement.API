using FluentValidation;
using LeaveManagement.API.Features.Administration.Dtos;

namespace LeaveManagement.API.Common.Validators.Administration
{
    public class CreateEmployeeRequestValidator
        : AbstractValidator<CreateEmployeeRequestDto>
    {
        public CreateEmployeeRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters.");

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .MaximumLength(100)
                .WithMessage("Full name cannot exceed 100 characters.");

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

            RuleFor(x => x.ManagerIds)
                .NotEmpty()
                .WithMessage("At least one manager must be assigned.");

            RuleForEach(x => x.ManagerIds)
                .GreaterThan(0)
                .WithMessage("Invalid manager ID.");
        }
    }
}