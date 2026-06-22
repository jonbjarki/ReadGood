using MediatR;
using Microsoft.AspNetCore.Identity;
using ReadGood.Domain.Entities;
using ReadGood.Infrastructure.Interfaces;

namespace ReadGood.Application.Features.Bookshelves.GetBookshelvesByUser
{
    public class GetBookshelvesByUserHandler : IRequestHandler<GetBookshelvesByUserQuery, List<GetBookshelvesByUserDto>?>
    {
        private readonly IBookshelfRepository _bookshelfRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetBookshelvesByUserHandler(
            IBookshelfRepository bookshelfRepository,
            UserManager<ApplicationUser> userManager)
        {
            _bookshelfRepository = bookshelfRepository;
            _userManager = userManager;
        }

        public async Task<List<GetBookshelvesByUserDto>?> Handle(GetBookshelvesByUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null)
            {
                Console.WriteLine($"User not found: {request.UserName}");
                return null;
            }
            Console.WriteLine($"User found: {user.UserName} (ID: {user.Id})");
            var bookshelves = await _bookshelfRepository.GetBookshelvesByUserId(user.Id, cancellationToken);

            return bookshelves
                .Select(bookshelf => new GetBookshelvesByUserDto
                {
                    Id = bookshelf.Id,
                    Name = bookshelf.Name,
                    UserId = bookshelf.UserId,
                    Description = bookshelf.Description,
                    IsDefaultShelf = bookshelf.IsDefaultShelf,
                    CreatedAt = bookshelf.CreatedAt
                })
                .ToList();
        }
    }
}
