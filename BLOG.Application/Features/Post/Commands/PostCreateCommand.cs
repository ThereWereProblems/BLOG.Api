using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.AppUser.Commands;
using BLOG.Application.Result;
using BLOG.Domain.DTO;
using BLOG.Domain.Model.ApplicationUser;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.Post.Commands
{
    public class PostCreateCommand : IRequest<Result<bool>>
    {
        public CreatePostDTO PostDTO { get; set; }
    }

    public class PostCreateCommandValidator : AbstractValidator<PostCreateCommand>
    {
        public PostCreateCommandValidator()
        {
            RuleFor(v => v.PostDTO.Title)
                .NotEmpty().WithMessage("Tytuł jest wymagany!")
                .MaximumLength(100).WithMessage("Maksymalna długość tytułu wynosi 100 znaków!");

            RuleFor(v => v.PostDTO.Description)
                .NotEmpty().WithMessage("Opis jest wymagany!")
                .MaximumLength(500).WithMessage("Maksymalna długość opisu wynosi 500 znaków!");

            RuleFor(v => v.PostDTO.Content)
                .NotEmpty().WithMessage("Treść artykułu jest wymagana");
        }
    }

    public class PostCreateCommandHandler : IRequestHandler<PostCreateCommand, Result<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostCreateCommandHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
            _contextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<Result<bool>> Handle(PostCreateCommand request, CancellationToken cancellationToken)
        {
            var entry = _mapper.Map<Domain.Model.Post.Post>(request.PostDTO);

            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);

            entry.UserId = user.Id;
            entry.PublishedAt = DateTime.Now;

            await _context.Posts.AddAsync(entry);
            await _context.SaveChangesAsync();
            
            return Result<bool>.Success(true);
        }
    }
}
