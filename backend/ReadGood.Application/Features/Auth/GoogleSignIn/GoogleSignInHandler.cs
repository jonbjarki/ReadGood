using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ReadGood.API.Configuration;
using ReadGood.Domain.Entities;
using ReadGood.Infrastructure.Interfaces;

namespace ReadGood.Application.Features.Auth.GoogleSignIn
{
    public class GoogleSignInHandler : IRequestHandler<GoogleSignInCommand, GoogleSignInResult>
    {
        private readonly IGoogleTokenValidator tokenValidator;
        private readonly UserManager<ApplicationUser> userManager;

        public GoogleSignInHandler(IGoogleTokenValidator tokenValidator, UserManager<ApplicationUser> userManager)
        {
            this.tokenValidator = tokenValidator;
            this.userManager = userManager;
        }

        public async Task<GoogleSignInResult> Handle(GoogleSignInCommand request, CancellationToken cancellationToken)
        {
            // Validate Token Using Google's Validator
            var googleUser = await tokenValidator.ValidateToken(request.idToken, cancellationToken);

            // Ensure email is verified
            if (!googleUser.EmailVerified)
            {
                // TODO: Throw custom exception
                throw new Exception("Email must be verified in Google before signing up");
            }

            // Check if user already exists in database
            var user = await userManager.FindByEmailAsync(googleUser.Email);

            // If user does not exist, create it
            if (user is null)
            {
                user = new ApplicationUser
                {
                    Email = googleUser.Email,
                    UserName = googleUser.Name ?? googleUser.Email,
                    EmailConfirmed = googleUser.EmailVerified
                };

                var res = await userManager.CreateAsync(user);
                if (!res.Succeeded)
                {
                    // TODO: Throw custom exception
                    throw new Exception("Failed to create account");
                }
            }
            else
            {
                // Check if user is already linked to this google account
                var logins = await userManager.GetLoginsAsync(user);
                var isLinked = logins.Any(login => login.LoginProvider == "Google" && login.ProviderKey == googleUser.Subject);

                // If not linked then add the link
                if (!isLinked)
                {
                    var loginInfo = new UserLoginInfo("Google", googleUser.Subject, "Google");

                    var linkResult = await userManager.AddLoginAsync(user, loginInfo);

                    if (!linkResult.Succeeded) {
                        // TODO: Throw custom exception
                        throw new Exception("Linking to account failed");
                    }
                }
            }

            // Generate new JWT token
            // TODO!

            // Return results along with token
            return new GoogleSignInResult
            {
                Email = googleUser.Email,
                UserId = user.Id,
                UserName = user.UserName ?? "",
                JwtToken = "example-token",
                ExpiresAt = new DateTimeOffset()
            };

        }
    }
}