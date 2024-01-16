using Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Web.Infrastructure;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is BusinessException businessException)
        {
            httpContext.Response.StatusCode = _exceptionStatusCodes
                .GetValueOrDefault(businessException.GetType(), StatusCodes.Status422UnprocessableEntity);

            await httpContext.Response.WriteAsJsonAsync(new ErrorResponse(businessException.Messages), cancellationToken);

            return true;
        }

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(new ErrorResponse(exception.Message), cancellationToken);

        return true;
    }

    private readonly Dictionary<Type, int> _exceptionStatusCodes = new()
    {
        { typeof(InvalidArgumentException), StatusCodes.Status400BadRequest },
        { typeof(InvalidCredentialsException), StatusCodes.Status401Unauthorized },
        { typeof(NotFoundException), StatusCodes.Status404NotFound },
    };
}

public class ErrorResponse(List<string> errors)
{
    public Response Errors { get; init; } = new(errors);

    public ErrorResponse(string message) : this([message]) { }

    public record Response(List<string> Body);
}
