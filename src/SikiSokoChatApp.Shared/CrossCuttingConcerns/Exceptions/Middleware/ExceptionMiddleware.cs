using System.Text.Json;
using SikiSokoChatApp.Shared.CrossCuttingConcerns.Exceptions.Handlers;
using SikiSokoChatApp.Shared.CrossCuttingConcerns.Logging.Serilog;
using Microsoft.AspNetCore.Http;

namespace SikiSokoChatApp.Shared.CrossCuttingConcerns.Exceptions.Middleware;

/// <summary>
/// Middleware for handling exceptions and logging them.
/// </summary>
public class ExceptionMiddleware
{
    private readonly HttpExceptionHandler _httpExceptionHandler;
    private readonly LoggerServiceBase _loggerService;
    private readonly RequestDelegate _next;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ExceptionMiddleware(RequestDelegate next, LoggerServiceBase loggerService)
    {
        _next = next;
        _loggerService = loggerService;
        _httpExceptionHandler = new HttpExceptionHandler();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context.Response, ex);
        }
    }

    private Task HandleExceptionAsync(HttpResponse response, Exception exception)
    {
        response.ContentType = "application/json";
        _httpExceptionHandler.Response = response;
        return _httpExceptionHandler.HandleExceptionAsync(exception);
    }
}