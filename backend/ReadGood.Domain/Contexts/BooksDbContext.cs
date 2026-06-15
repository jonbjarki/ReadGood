using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReadGood.Domain.Entities;

namespace ReadGood.Domain.Contexts
{
    public class BooksDbContext : IdentityDbContext<ApplicationUser>
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options) : base(options) { }

        public DbSet<Bookshelf> Bookshelves { get; set; } = null!;
        public DbSet<BookshelfBook> BookshelfBooks { get; set; } = null!;
    }
}