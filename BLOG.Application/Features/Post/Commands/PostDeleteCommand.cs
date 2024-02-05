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
    public class PostDeleteCommand : IRequest<Result<bool>>, ICacheCleanCommand
    {
        public int Id { get; set; }

        public string CacheGroup => $"{nameof(Domain.Model.Post.Post)}";
    }

    public class PostDeleteCommandValidator : AbstractValidator<PostDeleteCommand>
    {
        public PostDeleteCommandValidator()
        {
            RuleFor(v => v.Id)
           .NotEmpty().WithMessage("Id jest wymagane!");
        }
    }

    public class PostDeleteCommandHandler : IRequestHandler<PostDeleteCommand, Result<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly ICurentUserService _userService;

        public PostDeleteCommandHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext, ICurentUserService userService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
            _userService = userService;
        }

        public async Task<Result<bool>> Handle(PostDeleteCommand request, CancellationToken cancellationToken)
        {
            var entry = _context.Posts.FirstOrDefault(x => x.Id == request.Id);

            if (entry == null)
                return Result<bool>.NotFound();

            if (!(entry.UserId == _userService.UserId || _userService.IsAdmin))
                return Result<bool>.Forbidden();

            _context.Posts.Remove(entry);
            await _context.SaveChangesAsync();

            await _mediator.Send(new ImageDeleteCommand { FileName = entry.Image });

            return Result<bool>.Success(true);
        }
    }
}
