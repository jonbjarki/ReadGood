using ReadGood.Domain.DTOs;

namespace ReadGood.Application.Features.Books.GetBookByKey
{
    public class GetBookByKeyDto
    {
        public BookDetailsDto Book { get; set; } = null!;
    }
}