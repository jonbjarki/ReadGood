using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ReadGood.API.InputModels.Books
{
    public class GetBookMetadataInputModel
    {
        [FromQuery]
        [Required]
        public string Key { get; set; } = "";
    }
}