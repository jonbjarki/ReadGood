using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReadGood.Infrastructure.Exceptions;

namespace ReadGood.API.Errors;

public sealed class OpenLibraryExceptionHandler : IExceptionHandler
{
    private readonly ILogger<OpenLibraryExceptionHandler> _logger;

    public OpenLibraryExceptionHandler(ILogger<OpenLibraryExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not OpenLibraryApiException ex)
            return false;

        // Log full upstream details (ResponseContent may contain useful debugging info)
        _logger.LogWarning(exception,
            "OpenLibrary API failure. UpstreamStatus={UpstreamStatus} RequestUrl={RequestUrl} ResponseContent={ResponseContent}",
            ex.StatusCode,
            ex.RequestUrl,
            ex.ResponseContent);

        var (statusCode, title) = MapToHttpStatus(ex);

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = $"https://httpstatuses.com/{statusCode}",
            Detail = "Failed to retrieve data from upstream book provider."
        };

        // Safe details that help the frontend (don’t expose ResponseContent)
        problem.Extensions["provider"] = "OpenLibrary";
        if (ex.StatusCode is not null) problem.Extensions["upstreamStatus"] = ex.StatusCode;
        if (!string.IsNullOrWhiteSpace(ex.RequestUrl)) problem.Extensions["upstreamPath"] = ex.RequestUrl;

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    private static (int statusCode, string title) MapToHttpStatus(OpenLibraryApiException ex)
    {
        // If OpenLibrary returned a status code, map it appropriately
        return ex.StatusCode switch
        {
            400 => (502, "Upstream provider rejected the request"),   // your API likely built a bad query; still upstream failure to client
            401 or 403 => (502, "Upstream provider denied the request"),
            404 => (502, "Upstream resource not found"),
            408 => (504, "Upstream provider timed out"),
            429 => (503, "Upstream provider rate limited the request"),
            >= 500 and <= 599 => (502, "Upstream provider error"),
            _ when ex.StatusCode is not null => (502, "Upstream provider error"),
            _ => (502, "Upstream provider error")
        };
    }
}