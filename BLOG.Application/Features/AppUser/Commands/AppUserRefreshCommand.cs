using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLOG.Application.Result;
using BLOG.Domain.Model.ApplicationUser;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BLOG.Application.Features.AppUser.Commands
{
    public class AppUserRefreshCommand : IRequest<Result<IResult>>
    {
        public RefreshRequest Model { get; set; }
    }

    public class AppUserRefreshCommandValidator : AbstractValidator<AppUserRefreshCommand>
    {
        public AppUserRefreshCommandValidator()
        {
            RuleFor(v => v.Model)
                .NotNull().WithMessage("Model jest wymagany!");

            RuleFor(v => v.Model.RefreshToken)
                .NotEmpty().WithMessage("Token jest wymagany!");
        }
    }

    public class AppUserRefreshCommandHandler : IRequestHandler<AppUserRefreshCommand, Result<IResult>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected HttpContext _context;

        public AppUserRefreshCommandHandler(IMediator mediator, IMapper mapper, SignInManager<ApplicationUser> signInManager, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _mapper = mapper;
            _signInManager = signInManager;
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<IResult>> Handle(AppUserRefreshCommand request, CancellationToken cancellationToken)
        {
            _context = _httpContextAccessor.HttpContext;

            var bearerTokenOptions = _serviceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
            var timeProvider = _serviceProvider.GetRequiredService<TimeProvider>();

            var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
            var refreshTicket = refreshTokenProtector.Unprotect(request.Model.RefreshToken);

            // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation fails
            if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
                timeProvider.GetUtcNow() >= expiresUtc ||
                await _signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not ApplicationUser user)

            {
                return Result<IResult>.Unauthorized();
            }

            var newPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            var signIn = TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
            await signIn.ExecuteAsync(_context);
            return Result<IResult>.Success(signIn);

        }
    }
}