using FluentValidation;
using LeaveManagement.API.Features.Administration.Dtos;

namespace LeaveManagement.API.Common.Validators.Administration
{
    public class AssignManagerRequestValidator
        : AbstractValidator<AssignManagerRequestDto>
    {
        public AssignManagerRequestValidator()
        {
            RuleFor(x => x.ManagerIds)
                .NotEmpty()
                .WithMessage("At least one manager must be assigned.");

            RuleForEach(x => x.ManagerIds)
                .GreaterThan(0)
                .WithMessage("Invalid manager ID.");
        }
    }
}