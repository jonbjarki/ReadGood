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
                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status429TooManyRequests,
                    Title = "Google Books API Rate Limit Exceeded",
                    Type = "https://httpstatuses.com/429",
                    Detail = "The Google Books API rate limit has been exceeded. Please try again later."
                };
                httpContext.Response.StatusCode = problem.Status.Value;
                await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
                return true;
            }
            else if (exception is GoogleBooksApiException e)
            {
                var status = e.StatusCode ?? StatusCodes.Status500InternalServerError;
                var problem = new ProblemDetails
                {
                    Status = status,
                    Title = "Error from Google Books API",
                    Type = $"https://httpstatuses.com/{status}",
                    Detail = e.Message
                };
                _logger.LogError(e, "Handling Google Books API error: {Message}", e.Message);
                httpContext.Response.StatusCode = status;
                await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
                return true;
            }
            else
            {
                // Pass exception to the next handler
                return false;
            }
        }
    }
}