using AutoMapper;
using BLOG.Application.Result;
using BLOG.Domain.Model.ApplicationUser;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.AppUser.Commands
{
    public class AppUserLoginCommand : IRequest<Result<bool>>
    {
        public LoginRequest Login { get; set; }
        public bool? useCookies { get; set; }
        public bool? useSessionCookies { get; set; }
    }

    public class AppUserLoginCommandValidator : AbstractValidator<AppUserLoginCommand>
    {
        public AppUserLoginCommandValidator()
        {
            RuleFor(v => v.Login.Email)
                .NotEmpty().WithMessage("Email jest wymagany!")
                .EmailAddress().WithMessage("Podany Email jest nieprawidłowy!");

            RuleFor(v => v.Login.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane");
        }
    }

    public class AppUserLoginCommandHandler : IRequestHandler<AppUserLoginCommand, Result<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AppUserLoginCommandHandler(IMediator mediator, IMapper mapper, SignInManager<ApplicationUser> signInManager)
        {
            _mediator = mediator;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        public async Task<Result<bool>> Handle(AppUserLoginCommand request, CancellationToken cancellationToken)
        {

            var useCookieScheme = (request.useCookies == true) || (request.useSessionCookies == true);
            var isPersistent = (request.useCookies == true) && (request.useSessionCookies != true);
            _signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

            var result = await _signInManager.PasswordSignInAsync(request.Login.Email, request.Login.Password, isPersistent, lockoutOnFailure: true);

            if (result.RequiresTwoFactor)
            {
                if (!string.IsNullOrEmpty(request.Login.TwoFactorCode))
                {
                    result = await _signInManager.TwoFactorAuthenticatorSignInAsync(request.Login.TwoFactorCode, isPersistent, rememberClient: isPersistent);
                }
                else if (!string.IsNullOrEmpty(request.Login.TwoFactorRecoveryCode))
                {
                    result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(request.Login.TwoFactorRecoveryCode);
                }
            }

            if (!result.Succeeded)
            {
                return Result<bool>.Unauthorized();
            }

            // The signInManager already produced the needed response in the form of a cookie or bearer token.
            return Result<bool>.Success(true);
        }
    }
}
