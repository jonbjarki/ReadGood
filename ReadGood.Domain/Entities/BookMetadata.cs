using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadGood.Domain.Entities
{
    public class BookMetadata
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public DateOnly FirstPublished { get; set; }

        // public Author BookAuthor { get; set; } = null!;
        // public Review[] reviews { get; set; } = [];
        // public Genre[] Genres = [];

    }
}