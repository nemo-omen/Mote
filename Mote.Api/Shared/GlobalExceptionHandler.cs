using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Mote.Api.Shared;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
        context.Response.ContentType = "application/json";
        var statusCode = StatusCodes.Status500InternalServerError;
        var message = "An error occurred while processing your request.";

        switch (exception)
        {
            case ArgumentException:
                statusCode = StatusCodes.Status400BadRequest;
                message = exception.Message;
                break;
            case KeyNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                message = "Resource not found.";
                break;
            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status401Unauthorized;
                message = "Unauthorized access.";
                break;
            case InvalidOperationException:
                statusCode = StatusCodes.Status409Conflict;
                message = exception.Message;
                break;
        }
        
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = message,
            Detail = exception.Message,
            Instance = context.Request.Path
        };
        
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}