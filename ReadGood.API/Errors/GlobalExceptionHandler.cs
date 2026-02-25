using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ReadGood.API.Errors
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is NotFoundException e)
            {
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = $"{e.ResourceName} Not Found",
                    Type = "https://httpstatuses.com/404",
                    Detail = e.Message
                }, cancellationToken);
                return true;
            }
            else if (exception is OpenLibraryRateLimitExceededException)
            {
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status429TooManyRequests,
                    Title = "Open Library Rate Limit Exceeded",
                    Type = "https://httpstatuses.com/429",
                    Detail = "Too many requests have been made to the Open Library API. Please try again later."
                }, cancellationToken);
                return true;
            }
            else
            {
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred",
                    Type = "https://httpstatuses.com/500",
                    Detail = "An unexpected error occurred. Please try again later."
                }, cancellationToken);
                return true;
            }
        }
    }
}