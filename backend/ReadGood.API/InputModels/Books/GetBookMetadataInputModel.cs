using System.ComponentModel.DataAnnotations;

namespace ReadGood.API.InputModels.Books
{
    public class GetBookMetadataInputModel
    {
        [Required]
        public string Id { get; set; } = "";
    }
}