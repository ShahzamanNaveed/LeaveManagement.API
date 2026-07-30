using FluentValidation;
using LeaveManagement.API.Features.LeaveManagement.Dtos;

namespace LeaveManagement.API.Common.Validators.LeaveManagement
{
    public class ApplyLeaveRequestValidator
        : AbstractValidator<ApplyLeaveRequestDto>
    {
        public ApplyLeaveRequestValidator()
        {
            RuleFor(x => x.LeaveType)
                .IsInEnum()
                .WithMessage("Invalid leave type.");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("End date is required.");

            RuleFor(x => x)
                .Must(x => x.StartDate <= x.EndDate)
                .WithMessage("Start date cannot be after end date.");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason is required.")
                .MaximumLength(500)
                .WithMessage("Reason cannot exceed 500 characters.");
        }
    }
}