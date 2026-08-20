using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReadTogether.API.InputModels.Auth
{
    public class RegisterInputModel
    {

        [Required]
        [MinLength(3)]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email address is invalid")]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}