using System.Net;
using System.Text.Json;
using RestGateway.Exceptions;
using RestGateway.Models.DTOs.Responses;
using Microsoft.AspNetCore.Diagnostics;

namespace RestGateway.Middleware;

/// <summary>
/// Global exception handling middleware
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ApiErrorResponseDto
        {
            TraceId = context.TraceIdentifier
        };

        switch (exception)
        {
            case BusinessValidationException businessEx:
                response.Status = (int)HttpStatusCode.BadRequest;
                response.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                response.Title = "Validation Error";
                response.Detail = businessEx.Message;
                break;

            case ArgumentException argEx:
                response.Status = (int)HttpStatusCode.BadRequest;
                response.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                response.Title = "Bad Request";
                response.Detail = argEx.Message;
                break;

            case InvalidOperationException opEx:
                // Check if it's wrapping a business validation error
                if (opEx.InnerException is BusinessValidationException)
                {
                    response.Status = (int)HttpStatusCode.BadRequest;
                    response.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    response.Title = "Validation Error";
                    response.Detail = opEx.InnerException.Message;
                }
                else
                {
                    response.Status = (int)HttpStatusCode.BadGateway;
                    response.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.3";
                    response.Title = "Bad Gateway";
                    response.Detail = opEx.Message;
                }
                break;

            case KeyNotFoundException keyEx:
                response.Status = (int)HttpStatusCode.NotFound;
                response.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
                response.Title = "Not Found";
                response.Detail = keyEx.Message;
                break;

            case UnauthorizedAccessException:
                response.Status = (int)HttpStatusCode.Unauthorized;
                response.Type = "https://tools.ietf.org/html/rfc7235#section-3.1";
                response.Title = "Unauthorized";
                response.Detail = "You are not authorized to perform this action";
                break;

            case TimeoutException:
                response.Status = (int)HttpStatusCode.GatewayTimeout;
                response.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.5";
                response.Title = "Gateway Timeout";
                response.Detail = "The request to the upstream service timed out";
                break;

            default:
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                response.Title = "Internal Server Error";
                response.Detail = _environment.IsDevelopment()
                    ? exception.ToString()
                    : "An error occurred while processing your request";
                
                if (_environment.IsDevelopment())
                {
                    response.Extensions = new Dictionary<string, object>
                    {
                        { "exception", exception.GetType().Name },
                        { "stackTrace", exception.StackTrace ?? string.Empty }
                    };
                }
                break;
        }

        context.Response.StatusCode = response.Status;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _environment.IsDevelopment()
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
