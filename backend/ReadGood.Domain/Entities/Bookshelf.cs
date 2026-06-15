using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ReadGood.Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Bookshelf
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string UserId { get; set; }

        // Navigation properties
        public ApplicationUser User { get; set; } = null!;
        public ICollection<BookshelfBook> BookshelfBooks { get; set; } = new List<BookshelfBook>();
    }
}