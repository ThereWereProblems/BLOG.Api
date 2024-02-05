using AutoMapper;
using BLOG.Application.Caching;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.AppUser.Commands;
using BLOG.Application.Features.File.Commands;
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
    public class PostCreateCommand : IRequest<Result<int>>, ICacheCleanCommand
    {
        public CreatePostDTO PostDTO { get; set; }
        public IFormFile File { get; set; }

        public string CacheGroup => $"{nameof(Domain.Model.Post.Post)}";

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

            RuleFor(v => v.File)
                .NotNull().WithMessage("Zdjęcie jest wymagane");
        }
    }

    public class PostCreateCommandHandler : IRequestHandler<PostCreateCommand, Result<int>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly ICurentUserService _userService;

        public PostCreateCommandHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext, ICurentUserService curentUserService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
            _userService = curentUserService;
        }

        public async Task<Result<int>> Handle(PostCreateCommand request, CancellationToken cancellationToken)
        {
            var entry = _mapper.Map<Domain.Model.Post.Post>(request.PostDTO);

            entry.UserId = _userService.UserId;
            entry.PublishedAt = DateTime.Now;

            // zapis zdjęcia
            var result = await _mediator.Send(new ImageCreateCommand { File = request.File });

            if (!result.IsSuccess)
                return Result<int>.Invalid("Błąd podczas zapisywania zdjęcia!");

            entry.Image = result.Value!;

            await _context.Posts.AddAsync(entry);
            await _context.SaveChangesAsync();
            
            return Result<int>.Success(entry.Id);
        }
    }
}
