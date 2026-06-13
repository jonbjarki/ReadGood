using MediatR;
using Microsoft.AspNetCore.Identity;
using ReadGood.Domain.Entities;

namespace ReadGood.Application.Features.Users.GetUserByUserName
{
    public class GetUserByUserNameHandler : IRequestHandler<GetUserByUserNameQuery, GetUserByUserNameDto?>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public GetUserByUserNameHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserByUserNameDto?> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null)
            {
                return null;
            }

            return new GetUserByUserNameDto
            {
                DateJoined = user.DateCreated,
                UserName = user.UserName!,
                ImageUrl = user.ImageUrl
            };
        }
    }
}