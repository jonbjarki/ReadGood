using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ReadTogether.Domain.Entities
{
    [PrimaryKey(nameof(BookshelfId), nameof(VolumeId))]
    public class BookshelfBook
    {

        public int BookshelfId { get; set; }
        public required string VolumeId { get; set; }
        public required string Title { get; set; }
        public required string ThumbnailUrl { get; set; }

        // Navigation properties
        public Bookshelf Bookshelf { get; set; } = null!;
    }
}