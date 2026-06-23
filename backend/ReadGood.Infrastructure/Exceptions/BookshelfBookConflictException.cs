namespace ReadGood.Infrastructure.Exceptions
{
    [Serializable]
    public class BookshelfBookConflictException : Exception
    {
        public int BookshelfId { get; }
        public string VolumeId { get; }

        public BookshelfBookConflictException(int bookshelfId, string volumeId)
            : base($"Book with volume ID '{volumeId}' is already in bookshelf '{bookshelfId}'.")
        {
            BookshelfId = bookshelfId;
            VolumeId = volumeId;
        }

        public BookshelfBookConflictException(int bookshelfId, string volumeId, Exception innerException)
            : base($"Book with volume ID '{volumeId}' is already in bookshelf '{bookshelfId}'.", innerException)
        {
            BookshelfId = bookshelfId;
            VolumeId = volumeId;
        }
    }
}
