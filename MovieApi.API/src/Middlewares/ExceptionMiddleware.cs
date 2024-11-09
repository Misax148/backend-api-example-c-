using System.Net;
using System.Text.Json;
using MovieApi.Core.src.Domain.Exceptions;
using FluentValidation;

namespace MovieApi.API.src.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env) =>
        (_next, _logger, _env) = (next, logger, env);

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, response) = exception switch
        {
            ValidationException ex => (HttpStatusCode.BadRequest, new ErrorDetails
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Validation failed",
                ErrorCode = "VALIDATION_ERROR",
                TimeStamp = DateTime.UtcNow,
                Details = ex.Errors.Select(e => new ValidationError 
                { 
                    PropertyName = e.PropertyName, 
                    ErrorMessage = e.ErrorMessage 
                }).ToList()
            }),
            ApiException ex => (ex.StatusCode, new ErrorDetails
            {
                StatusCode = (int)ex.StatusCode,
                Message = ex.Message,
                ErrorCode = ex.ErrorCode,
                TimeStamp = ex.TimeStamp,
                Details = new List<ValidationError>()
            }),
            _ => (HttpStatusCode.InternalServerError, new ErrorDetails
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = _env.IsDevelopment() ? exception.Message : "An unexpected error occurred",
                ErrorCode = "INTERNAL_SERVER_ERROR",
                TimeStamp = DateTime.UtcNow,
                Details = _env.IsDevelopment() 
                    ? new List<ValidationError> { new() { PropertyName = "StackTrace", ErrorMessage = exception.StackTrace } }
                    : new List<ValidationError>()
            })
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsJsonAsync(response, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        });
    }
}