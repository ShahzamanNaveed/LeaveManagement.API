using FluentValidation;
using LeaveManagement.API.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LeaveManagement.API.Common.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null)
                    continue;

                var validatorType =
                    typeof(IValidator<>).MakeGenericType(argument.GetType());

                var validator =
                    _serviceProvider.GetService(validatorType);

                if (validator == null)
                    continue;

                var validationContext =
                    new ValidationContext<object>(argument);

                var validationResult =
                    await ((IValidator)validator)
                        .ValidateAsync(validationContext);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                        .ToList();

                    throw new BadRequestException(
                        "Validation failed.",
                        errors);
                }
            }

            await next();
        }
    }
}