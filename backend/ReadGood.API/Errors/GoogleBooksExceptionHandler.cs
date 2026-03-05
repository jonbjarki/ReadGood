using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReadGood.Infrastructure.Exceptions;

namespace ReadGood.API.Errors
{
    public sealed class GoogleBooksExceptionHandler : IExceptionHandler
    {

        private readonly ILogger<GoogleBooksExceptionHandler> _logger;

        public GoogleBooksExceptionHandler(ILogger<GoogleBooksExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is GoogleBooksRateLimitExceededException)
            {
                _logger.LogWarning("Handling Google Books API rate limit exceeded error.");
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status429TooManyRequests,
                    Title = "Google Books API Rate Limit Exceeded",
                    Type = "https://httpstatuses.com/429",
                    Detail = "The Google Books API rate limit has been exceeded. Please try again later."
                }, cancellationToken);
                return true;
            }
            else if (exception is NotFoundException ex)
            {
                _logger.LogWarning("Handling NotFoundException for {ResourceName} with ID {ResourceId}", ex.ResourceName, ex.ResourceId);
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = $"{ex.ResourceName} Not Found",
                    Type = "https://httpstatuses.com/404",
                    Detail = ex.Message
                }, cancellationToken);
                return true;
            }
            else if (exception is GoogleBooksApiException e)
            {
                var status = e.StatusCode ?? StatusCodes.Status500InternalServerError;
                _logger.LogError(e, "Handling Google Books API error: {Message}", e.Message);
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = status,
                    Title = "Error from Google Books API",
                    Type = $"https://httpstatuses.com/{status}",
                    Detail = e.Message
                }, cancellationToken);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}