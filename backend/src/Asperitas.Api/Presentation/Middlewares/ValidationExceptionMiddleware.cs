using Asperitas.Api.Presentation.Contracts.Responses;
using FluentValidation;

namespace Asperitas.Api.Presentation.Middlewares;

public class ValidationExceptionMiddleware(RequestDelegate request)
{
    private readonly RequestDelegate _request = request;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _request(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = 400;
            var messages = exception.Errors.Select(x => x.ErrorMessage).ToList();
            var errorResponse = new ErrorResponse
            {
                Message = "Validation failed",
                Errors = messages
            };
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (KeyNotFoundException exception)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new ErrorResponse { Message = exception.Message });
        }
        catch (UnauthorizedAccessException exception)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new ErrorResponse { Message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new ErrorResponse { Message = exception.Message });
        }
        catch (ArgumentException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorResponse { Message = exception.Message });
        }
    }
}
