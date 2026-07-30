using FluentValidation;
using LeaveManagement.API.Features.FiscalYears.Dtos;

namespace LeaveManagement.API.Common.Validators.FiscalYears
{
    public class CreateFiscalYearSettingsValidator
        : AbstractValidator<CreateFiscalYearSettingsDto>
    {
        public CreateFiscalYearSettingsValidator()
        {
            RuleFor(x => x.StartMonth)
                .InclusiveBetween(1, 12)
                .WithMessage("Start month must be between 1 and 12.");

            RuleFor(x => x.StartDay)
                .InclusiveBetween(1, 31)
                .WithMessage("Start day must be between 1 and 31.");
        }
    }
}