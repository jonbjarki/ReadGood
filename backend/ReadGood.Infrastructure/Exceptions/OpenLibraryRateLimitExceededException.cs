[System.Serializable]
public class OpenLibraryRateLimitExceededException : System.Exception
{
    public OpenLibraryRateLimitExceededException() { }
    public OpenLibraryRateLimitExceededException(string message) : base(message) { }
    public OpenLibraryRateLimitExceededException(string message, Exception inner) : base(message, inner) { }
}