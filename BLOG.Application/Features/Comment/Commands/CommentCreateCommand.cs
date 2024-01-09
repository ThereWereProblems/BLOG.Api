using AutoMapper;
using BLOG.Application.Caching;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.Comment.Commands;
using BLOG.Application.Result;
using BLOG.Domain.DTO;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.Comment.Commands
{
    public class CommentCreateCommand : IRequest<Result<int>>, ICacheCleanCommand
    {
        public CreateCommentDTO CommentDTO { get; set; }

        public string CacheGroup => $"{nameof(Domain.Model.Comment.Comment)}_{CommentDTO.PostId}";
    }

    public class CommentCreateCommandValidator : AbstractValidator<CommentCreateCommand>
    {
        public CommentCreateCommandValidator()
        {
            RuleFor(v => v.CommentDTO.PostId)
                .NotEmpty().WithMessage("Klucz wpisu jest wymagany!");

            RuleFor(v => v.CommentDTO.Content)
                .NotEmpty().WithMessage("Treść artykułu jest wymagana")
                .MaximumLength(500).WithMessage("Maksymalna długość komentarza wynosi 500 znaków!");
        }
    }

    public class CommentCreateCommandHandler : IRequestHandler<CommentCreateCommand, Result<int>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly ICurentUserService _userService;

        public CommentCreateCommandHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext, ICurentUserService curentUserService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
            _userService = curentUserService;
        }

        public async Task<Result<int>> Handle(CommentCreateCommand request, CancellationToken cancellationToken)
        {
            var entry = _mapper.Map<Domain.Model.Comment.Comment>(request.CommentDTO);

            entry.UserId = _userService.UserId;
            entry.PublishedAt = DateTime.Now;

            await _context.Comments.AddAsync(entry);
            await _context.SaveChangesAsync();

            return Result<int>.Success(entry.Id);
        }
    }
}
