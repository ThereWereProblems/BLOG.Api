using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.Comment.Commands;
using BLOG.Application.Result;
using BLOG.Application.Wrappers;
using BLOG.Domain.ReadModel;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.Comment.Queries
{
    public class CommentSearchQuery : BaseCacheableQuery<Result<PagedList<CommentSearchResult>>>
    {
        public int PostId { get; set; }

        public override string CacheKey => $"{nameof(CommentSearchQuery)}_{PostId}_{PageIndex}_{PageSize}";
        public override string CacheGroup => $"{nameof(Domain.Model.Comment.Comment)}";
    }

    public class CommentSearchQueryValidator : AbstractValidator<CommentSearchQuery>
    {
        public CommentSearchQueryValidator()
        {
            RuleFor(v => v.PostId)
           .NotEmpty().WithMessage("Id jest wymagane!");
        }
    }

    public class CommentSearchQueryHandler : IRequestHandler<CommentSearchQuery, Result<PagedList<CommentSearchResult>>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CommentSearchQueryHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
        }

        public async Task<Result<PagedList<CommentSearchResult>>> Handle(CommentSearchQuery request, CancellationToken cancellationToken)
        {
            if (request.PageIndex < 1)
            {
                return Result<PagedList<CommentSearchResult>>.Invalid("Błędny indeks strony!");
            }

            if (request.PageSize < 1)
            {
                return Result<PagedList<CommentSearchResult>>.Invalid("Błędna wielkość strony!");
            }

            var pager = new DataPager(request.PageIndex, request.PageSize);
            pager.TotalRecords = await _context.Comments.CountAsync(cancellationToken);
            pager.TotalPages = (int)Math.Ceiling((double)pager.TotalRecords / (double)request.PageSize);


            IQueryable<Domain.Model.Comment.Comment> query = _context.Comments.Where(x => x.PostId == request.PostId).Include(x => x.User).OrderByDescending(x => x.PublishedAt);
            var pagedList = query.Skip(pager.PageSize * (pager.PageIndex - 1)).Take(pager.PageSize).ToList();

            var result = pagedList.Select(x => _mapper.Map<CommentSearchResult>(x)).ToList();

            return Result<PagedList<CommentSearchResult>>.Success(new PagedList<CommentSearchResult>(pager, result));
        }
    }
}
