using FluentValidation;
using LeaveManagement.API.Features.Management.Dtos;

namespace LeaveManagement.API.Common.Validators.Management
{
    public class CreateManagerRequestValidator
        : AbstractValidator<CreateManagerRequestDto>
    {
        public CreateManagerRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.Department)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Designation)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}