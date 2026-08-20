using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadTogether.API.InputModels.Bookshelves
{
    public class AddBookToBookshelfInputModel
    {
        public required string Title { get; set; }
        public required string ThumbnailUrl { get; set; }
    }
}