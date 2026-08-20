using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadTogether.Application.Features.Bookshelves.CreateBookshelf
{
    public class CreateBookshelfDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string UserId { get; set; }
    }
}