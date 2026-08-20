using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadTogether.API.InputModels.Books
{
    public class CreateUserInputModel
    {
        public required string Email { get; set; }
    }
}