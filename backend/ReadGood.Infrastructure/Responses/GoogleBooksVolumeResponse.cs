namespace ReadGood.Infrastructure.Responses
{
    /// <summary>
    /// Response from the Google Books API when fetching a single volume
    /// Uses the Volume class from GoogleBooksSearchResponse since the structure is identical.
    /// </summary>
    public class GoogleBooksVolumeResponse : Volume
    {
        // This class inherits all properties from Volume
        // The Google Books volume endpoint returns the same Volume structure
    }
}
