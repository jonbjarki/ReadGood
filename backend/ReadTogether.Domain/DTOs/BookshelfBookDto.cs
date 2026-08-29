using System;
using System.Collections.Generic;
using System.Text;

namespace ReadTogether.Domain.DTOs
{
    public class BookshelfBookDto
    {
        public string VolumeId { get; set; } = null!;
        public string? Title { get; set; }
        public string? ThumbnailUrl { get; set; }

    }
}
