using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Result;
using BLOG.Application.Wrappers;
using BLOG.Domain.ReadModel;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.AppUser.Queries
{
    public class AppUserGetInfoQuery : IRequest<Result<UserInfoResult>>
    {

    }

    public class AppUserGetInfoQueryHandler : IRequestHandler<AppUserGetInfoQuery, Result<UserInfoResult>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly ICurentUserService _userService;

        public AppUserGetInfoQueryHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext, ICurentUserService userService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
            _userService = userService;
        }

        public async Task<Result<UserInfoResult>> Handle(AppUserGetInfoQuery request, CancellationToken cancellationToken)
        {
            var userId = _userService.UserId;

            var roles = _userService.Roles;

            var result = _mapper.Map<UserInfoResult>(await _context.Users.FirstOrDefaultAsync(x => x.Id == userId));
            result.Roles = roles;

            return Result<UserInfoResult>.Success(result);
        }
    }
}
