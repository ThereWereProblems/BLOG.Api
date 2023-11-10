using AutoMapper;
using BLOG.Domain.DTO;
using BLOG.Domain.Model.ApplicationUser;
using BLOG.Infrastructure.Persistance;
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
    public class AppUserRegisterCommand : IRequest<bool>
    {
        public RegisterAppUserDTO UserDTO { get; set; }
    }

    public class AppUserRegisterCommandValidator : AbstractValidator<AppUserRegisterCommand>
    {
        private readonly AppDbContext _dbContext;

        public AppUserRegisterCommandValidator(AppDbContext context)
        {
            _dbContext = context;

            //RuleFor(v => v.UserDTO.FirstName)
            //    .NotEmpty().WithMessage("Imię jest wymagane");

            //RuleFor(v => v.UserDTO.LastName)
            //    .NotEmpty().WithMessage("Nazwisko jest wymagane");

            //RuleFor(v => v.UserDTO.Email)
            //    .NotEmpty().WithMessage("Email jest wymagany");

            //RuleFor(v => v.UserDTO.Password)
            //    .NotEmpty().WithMessage("Hasło jest wymagane")
            //    .MinimumLength(8).WithMessage("Minimalna długość hasła wynosi 8 znaków");

            RuleFor(v => v.UserDTO.NickName)
                .MustAsync((model, nick, cancellationToken) => IsNickTaken(nick))
                .WithMessage("Podany Nick jest już zajęty");
        }

        private async Task<bool> IsNickTaken(string nick)
        {
            // sprawdza w bazie czy nick jest już zajęty
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.NickName == nick) == null;
        }
    }

    public class AppUserRegisterCommandHandler : IRequestHandler<AppUserRegisterCommand, bool>
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

        public async Task<bool> Handle(AppUserRegisterCommand request, CancellationToken cancellationToken)
        {
            var emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
            var email = request.UserDTO.Email;

            //if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
            //{
            //    return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
            //}

            var user = _mapper.Map<ApplicationUser>(request.UserDTO);
            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, request.UserDTO.Password);

            if (!result.Succeeded)
            {
                return false;
                //return CreateValidationProblem(result);
            }

            //await SendConfirmationEmailAsync(user, userManager, context, email);
            return true;
        }
    }
}
