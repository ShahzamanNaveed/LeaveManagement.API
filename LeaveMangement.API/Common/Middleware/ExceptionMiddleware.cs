using System.Text.Json;
using LeaveManagement.API.Common.Exceptions;

namespace LeaveManagement.API.Common.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode = exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            object response;

            if (exception is BadRequestException badRequestException
                && badRequestException.Errors != null
                && badRequestException.Errors.Any())
            {
                response = new
                {
                    StatusCode = statusCode,
                    Message = badRequestException.Message,
                    Errors = badRequestException.Errors
                };
            }
            else
            {
                response = new
                {
                    StatusCode = statusCode,
                    Message = exception.Message
                };
            }

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}