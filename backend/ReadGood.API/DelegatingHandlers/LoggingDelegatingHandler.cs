using System.Collections.Frozen;
using Microsoft.AspNetCore.WebUtilities;

namespace ReadGood.API.Handlers
{
    public class LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger, IConfiguration configuration) : DelegatingHandler
    {
        /// <summary>
        /// Intercepts outgoing HTTP requests to the Google Books API, adding the API key as a query parameter and logging request details.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
        {
            try
            {
                using (logger.BeginScope("Handling HTTP request to {Url}", request.RequestUri))
                {
                    logger.LogInformation("==== STARTING HANDLING HTTP REQUEST ====");
                    //logger.LogInformation("With headers: {Headers}", request.Headers.ToFrozenDictionary(h => h.Key, h => string.Join(", ", h.Value)));
                    if (request.RequestUri is null)
                    {
                        logger.LogError("Request URI is null");
                        throw new InvalidOperationException("Request URI cannot be null");
                    }

                    // Add the API key as a query parameter to the outgoing request
                    var oldUri = request.RequestUri.ToString();
                    var queryParams = new Dictionary<string, string?>
                {
                    { "key", configuration["GoogleBooksApiKey"] ?? throw new InvalidOperationException("GoogleBooksApiKey is not configured") }
                };
                    var newUri = QueryHelpers.AddQueryString(oldUri, queryParams);
                    request.RequestUri = new Uri(newUri);

                    if (logger.IsEnabled(LogLevel.Information))
                    {
                        var censoredKey = string.Concat(request.RequestUri.ToString().AsSpan(0, 5), "****");
                        logger.LogInformation("Modified request URL to include API key: {Url}", censoredKey);
                    }

                    // Forwards the modified request
                    var result = await base.SendAsync(request, cancellationToken);

                    logger.LogInformation("==== FINISHED HANDLING HTTP REQUEST ====");
                    return result;
                }

            }
            catch (Exception e)
            {
                logger.LogError(e, "HTTP request failed");

                throw;
            }
        }
    }
}