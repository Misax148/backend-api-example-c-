using System.Net;
using System.Text.Json;
using MovieApi.Core.src.Domain.Exceptions;
using FluentValidation;

namespace MovieApi.API.src.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

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
                Message = "An unexpected error occurred",
                ErrorCode = "INTERNAL_SERVER_ERROR",
                TimeStamp = DateTime.UtcNow,
                Details = new List<ValidationError>()
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