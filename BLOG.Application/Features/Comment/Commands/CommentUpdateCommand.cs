using AutoMapper;
using BLOG.Application.Caching;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Result;
using BLOG.Domain.DTO;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.Comment.Commands
{
    public class CommentUpdateCommand : IRequest<Result<bool>>, ICacheCleanCommand
    {
        public int Id { get; set; }

        public CreateCommentDTO CommentDTO { get; set; }

        public string CacheGroup => $"{nameof(Domain.Model.Comment.Comment)}";
    }

    public class CommentUpdateCommandValidator : AbstractValidator<CommentUpdateCommand>
    {
        public CommentUpdateCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id jest wymagane!");

            RuleFor(v => v.CommentDTO.Content)
                .NotEmpty().WithMessage("Treść artykułu jest wymagana")
                .MaximumLength(500).WithMessage("Maksymalna długość komentarza wynosi 500 znaków!");
        }
    }

    public class CommentUpdateCommandHandler : IRequestHandler<CommentUpdateCommand, Result<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly ICurentUserService _userService;

        public CommentUpdateCommandHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext, ICurentUserService userService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
            _userService = userService;
        }

        public async Task<Result<bool>> Handle(CommentUpdateCommand request, CancellationToken cancellationToken)
        {
            var entry = _context.Comments.FirstOrDefault(x => x.Id == request.Id);

            if (entry == null)
                return Result<bool>.NotFound();

            if (entry.UserId != _userService.UserId)
                return Result<bool>.Forbidden();

            entry.Content = request.CommentDTO.Content;

            _context.Comments.Update(entry);
            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
