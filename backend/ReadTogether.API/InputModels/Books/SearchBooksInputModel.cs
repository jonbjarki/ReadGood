using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ReadTogether.API.InputModels.Books
{
    public class SearchBooksInputModel : IValidatableObject
    {
        [FromQuery]
        [Required]
        public string Title { get; set; } = "";

        [FromQuery]
        public string? Author { get; set; }

        [FromQuery]
        public string? Subject { get; set; }

        [FromQuery]
        public int Page { get; set; } = 1;

        [FromQuery]
        [Range(1, 30)]
        public int PageSize { get; set; } = 10;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool hasTitle = !string.IsNullOrWhiteSpace(Title);            
            bool hasAuthor = !string.IsNullOrWhiteSpace(Author);
            bool hasSubject = !string.IsNullOrWhiteSpace(Subject);
            if (!hasTitle && !hasAuthor && !hasSubject)
            {
                yield return new ValidationResult(
                    "At least one of the following fields must be provided: Title, Author, Subject.",
                    [nameof(Title), nameof(Author), nameof(Subject)]);
            }
        }
    }
}