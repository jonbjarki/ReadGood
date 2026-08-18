using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ReadTogether.Application.Features.Users.GetUserByUserName
{
    public record GetUserByUserNameQuery(string UserName) : IRequest<GetUserByUserNameDto?>;
}