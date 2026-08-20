using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadTogether.Domain.DTOs
{
    public class GoogleUserInfoDto
    {
        public required string Subject { get; set; }
        public required string Email { get; set; }
        public required bool EmailVerified { get; set; }
        public string? Name { get; set; }
        public string? PictureUrl { get; set; }
    }
}