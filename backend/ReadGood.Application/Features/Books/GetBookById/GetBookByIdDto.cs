using ReadGood.Domain.DTOs;

namespace ReadGood.Application.Features.Books.GetBookById
{
    public class GetBookByIdDto
    {
        public BookDetailsDto Book { get; set; } = null!;
    }
}