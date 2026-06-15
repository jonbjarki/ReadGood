using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadGood.Application.Features.Bookshelves.CreateBookshelf
{
    public class CreateBookshelfDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string UserId { get; set; }
    }
}