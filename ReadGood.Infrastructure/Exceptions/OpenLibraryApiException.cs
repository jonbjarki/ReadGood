namespace ReadGood.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception thrown when an OpenLibrary API request fails.
    /// </summary>
    public class OpenLibraryApiException : Exception
    {
        public string? RequestUrl { get; }
        public int? StatusCode { get; }
        public string? ResponseContent { get; }

        public OpenLibraryApiException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }

        public OpenLibraryApiException(
            string message,
            string? requestUrl,
            int? statusCode = null,
            string? responseContent = null,
            Exception? innerException = null)
            : base(message, innerException)
        {
            RequestUrl = requestUrl;
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }
    }
}
