using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadGood.Infrastructure.Exceptions
{
    public class GoogleBooksRateLimitExceededException : Exception
    {
        public GoogleBooksRateLimitExceededException() { }
    public GoogleBooksRateLimitExceededException(string message) : base(message) { }
    public GoogleBooksRateLimitExceededException(string message, Exception inner) : base(message, inner) { }
    }
}