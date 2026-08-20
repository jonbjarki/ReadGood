using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadTogether.API.InputModels.Auth
{
    public class GoogleSignInInputModel
    {
        public required string IdToken { get; set; }
    }
}