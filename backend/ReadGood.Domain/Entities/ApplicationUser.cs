using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ReadGood.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? ProfilePicture {get; set;}
    }
}