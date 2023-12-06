using AutoMapper;
using BLOG.Application.Common.Abstractions;
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
    public class PostUpdateCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public CreatePostDTO PostDTO { get; set; }
    }

    public class PostUpdateCommandValidator : AbstractValidator<PostUpdateCommand>
    {
        public PostUpdateCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id jest wymagane!");

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

    public class PostUpdateCommandHandler : IRequestHandler<PostUpdateCommand, Result<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostUpdateCommandHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
            _contextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<Result<bool>> Handle(PostUpdateCommand request, CancellationToken cancellationToken)
        {
            var entry = _context.Posts.FirstOrDefault(x => x.Id == request.Id);

            if (entry == null)
                return Result<bool>.NotFound();

            entry.Title = request.PostDTO.Title;
            entry.Description = request.PostDTO.Description;
            entry.Content = request.PostDTO.Content;

            _context.Posts.Update(entry);
            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
