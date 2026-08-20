using ReadTogether.Domain.DTOs;

namespace ReadTogether.Application.Features.Books.GetBookById
{
    public class GetBookByIdDto
    {
        public BookDetailsDto Book { get; set; } = null!;
    }
}