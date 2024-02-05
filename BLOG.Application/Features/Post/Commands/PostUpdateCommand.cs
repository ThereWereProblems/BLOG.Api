using AutoMapper;
using BLOG.Application.Caching;
using BLOG.Application.Common.Abstractions;
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
    public class PostUpdateCommand : IRequest<Result<bool>>, ICacheCleanCommand
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }
        public CreatePostDTO PostDTO { get; set; }

        public string CacheGroup => $"{nameof(Domain.Model.Post.Post)}";
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
        private readonly ICurentUserService _userService;

        public PostUpdateCommandHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext, ICurentUserService userService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
            _userService = userService;
        }

        public async Task<Result<bool>> Handle(PostUpdateCommand request, CancellationToken cancellationToken)
        {
            var entry = _context.Posts.FirstOrDefault(x => x.Id == request.Id);

            if (entry == null)
                return Result<bool>.NotFound();

            if (!(entry.UserId == _userService.UserId || _userService.IsAdmin))
                return Result<bool>.Forbidden();

            var imageToDelete = entry.Image;
            if (request.File != null)
            {
                var result = await _mediator.Send(new ImageCreateCommand { File = request.File });

                if (!result.IsSuccess)
                    return Result<bool>.Error(result.Errors.First());

                entry.Image = result.Value;
            }

            entry.Title = request.PostDTO.Title;
            entry.Description = request.PostDTO.Description;
            entry.Content = request.PostDTO.Content;

            _context.Posts.Update(entry);
            await _context.SaveChangesAsync();

            if (request.File != null)
                await _mediator.Send(new ImageDeleteCommand { FileName = imageToDelete });

            return Result<bool>.Success(true);
        }
    }
}
