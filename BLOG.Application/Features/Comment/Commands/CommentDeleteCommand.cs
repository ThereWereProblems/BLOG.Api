using AutoMapper;
using BLOG.Application.Caching;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.Comment.Events;
using BLOG.Application.Result;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.Comment.Commands
{
    public class CommentDeleteCommand : IRequest<Result<bool>>, ICacheCleanCommand
    {
        public int Id { get; set; }

        public string CacheGroup => $"{nameof(Domain.Model.Comment.Comment)}";
    }

    public class CommentDeleteCommandValidator : AbstractValidator<CommentDeleteCommand>
    {
        public CommentDeleteCommandValidator()
        {
            RuleFor(v => v.Id)
           .NotEmpty().WithMessage("Id jest wymagane!");
        }
    }

    public class CommentDeleteCommandHandler : IRequestHandler<CommentDeleteCommand, Result<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly ICurentUserService _userService;

        public CommentDeleteCommandHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext, ICurentUserService userService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
            _userService = userService;
        }

        public async Task<Result<bool>> Handle(CommentDeleteCommand request, CancellationToken cancellationToken)
        {
            var entry = _context.Comments.FirstOrDefault(x => x.Id == request.Id);

            if (entry == null)
                return Result<bool>.NotFound();

            if (entry.UserId != _userService.UserId)
                return Result<bool>.Forbidden();

            _context.Comments.Remove(entry);
            await _context.SaveChangesAsync();

            await _mediator.Publish(new CommentChangedEvent { PostId = entry.PostId });

            return Result<bool>.Success(true);
        }
    }
}
