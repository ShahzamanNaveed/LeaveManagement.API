using FluentValidation;
using LeaveManagement.API.Features.FiscalYears.Dtos;

namespace LeaveManagement.API.Common.Validators.FiscalYears
{
    public class CreateFiscalYearValidator
        : AbstractValidator<CreateFiscalYearDto>
    {
        public CreateFiscalYearValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Fiscal year name is required.")
                .MaximumLength(50)
                .WithMessage("Fiscal year name cannot exceed 50 characters.");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("End date is required.");

            RuleFor(x => x)
                .Must(x => x.StartDate <= x.EndDate)
                .WithMessage("Start date cannot be after end date.");
        }
    }
}