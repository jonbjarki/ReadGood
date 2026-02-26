using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ReadGood.API.InputModels.Books
{
    public class SearchBooksInputModel
    {
        [FromQuery]
        [Required]
        public string Title { get; set; } = "";

        [FromQuery]
        public int Page { get; set; } = 1;

        [FromQuery]
        [Range(1, 20)]
        public int PageSize { get; set; } = 10;
    }
}