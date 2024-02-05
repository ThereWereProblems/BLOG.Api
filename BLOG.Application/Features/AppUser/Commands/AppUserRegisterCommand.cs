using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Result;
using BLOG.Domain.DTO;
using BLOG.Domain.Model.ApplicationUser;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.AppUser.Commands
{
    public class AppUserRegisterCommand : IRequest<Result<bool>>
    {
        public RegisterAppUserDTO UserDTO { get; set; }
    }

    public class AppUserRegisterCommandValidator : AbstractValidator<AppUserRegisterCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public AppUserRegisterCommandValidator(IApplicationDbContext context)
        {
            _dbContext = context;

            RuleFor(v => v.UserDTO.Email)
                .NotEmpty().WithMessage("Email jest wymagany!")
                .EmailAddress().WithMessage("Podany Email jest nieprawidłowy!");

            RuleFor(v => v.UserDTO.NickName)
                .MustAsync((model, nick, cancellationToken) => IsNickTaken(nick))
                .WithMessage("Podany Nick jest już zajęty!");
        }

        private async Task<bool> IsNickTaken(string nick)
        {
            // sprawdza w bazie czy nick jest już zajęty
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.NickName == nick) == null;
        }
    }

    public class AppUserRegisterCommandHandler : IRequestHandler<AppUserRegisterCommand, Result<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;

        public AppUserRegisterCommandHandler(IMediator mediator, IMapper mapper, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userManager = userManager;
            _userStore = userStore;
        }

        public async Task<Result<bool>> Handle(AppUserRegisterCommand request, CancellationToken cancellationToken)
        {
            var emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
            var email = request.UserDTO.Email;

            var user = _mapper.Map<ApplicationUser>(request.UserDTO);
            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, request.UserDTO.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => new AppProblemDetail("", x.Description)).ToList();
                return Result<bool>.Invalid(errors);
            }

            _userManager.AddToRoleAsync(user, "Member");

            //await SendConfirmationEmailAsync(user, userManager, context, email);
            return Result<bool>.Success(true);
        }
    }
}
