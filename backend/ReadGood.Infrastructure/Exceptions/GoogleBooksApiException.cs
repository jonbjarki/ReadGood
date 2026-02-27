using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadGood.Infrastructure.Exceptions
{
    namespace ReadGood.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception thrown when an Google Books API request fails.
    /// </summary>
    public class GoogleBooksApiException : Exception
    {
        public string? RequestUrl { get; }
        public int? StatusCode { get; }
        public string? ResponseContent { get; }

        public GoogleBooksApiException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }

        public GoogleBooksApiException(
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

}