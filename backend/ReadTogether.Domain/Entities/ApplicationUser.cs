using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ReadTogether.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateCreated { get; set; }
        public bool ProfileCompleted { get; set; } = false; // Whether the user has filled out all required profile fields (username, email, etc.)
        public string? ImageUrl { get; set; }
    }
}