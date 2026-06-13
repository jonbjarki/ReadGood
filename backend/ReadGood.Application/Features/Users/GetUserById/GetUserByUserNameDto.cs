using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadGood.Application.Features.Users.GetUserByUserName
{
    public class GetUserByUserNameDto
    {
        public string UserName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public DateTime DateJoined { get; set; }
    }
}