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
            if (exception is GoogleBooksApiException e)
            {
                var status = e.StatusCode ?? StatusCodes.Status500InternalServerError;
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