using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ReadGood.Domain.Entities
{
    [Index(nameof(Name), nameof(UserId), IsUnique = true)]
    public class Bookshelf
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string UserId { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDefaultShelf { get; set; } = false;

        // Navigation properties
        public ApplicationUser User { get; set; } = null!;
        public ICollection<BookshelfBook> BookshelfBooks { get; set; } = new List<BookshelfBook>();
    }
}