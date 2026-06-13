using MediatR;
using Microsoft.AspNetCore.Identity;
using ReadGood.Domain.Entities;

namespace ReadGood.Application.Features.Users.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserDto?>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateUserHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UpdateUserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user is null)
            {
                return null;
            }

            if (request.UserName is not null)
            {
                user.UserName = request.UserName.Trim();
            }

            if (request.Email is not null)
            {
                user.Email = request.Email.Trim();
            }

            user.ProfileCompleted = !string.IsNullOrWhiteSpace(user.UserName)
                && !string.IsNullOrWhiteSpace(user.Email);

            await _userManager.UpdateAsync(user);

            return new UpdateUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                ProfileCompleted = user.ProfileCompleted
            };
        }
    }
}