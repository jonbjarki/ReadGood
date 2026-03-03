namespace ReadGood.Infrastructure.Exceptions
{
    public class GoogleBooksRateLimitExceededException : Exception
    {
        public GoogleBooksRateLimitExceededException() { }
    public GoogleBooksRateLimitExceededException(string message) : base(message) { }
    public GoogleBooksRateLimitExceededException(string message, Exception inner) : base(message, inner) { }
    }
}