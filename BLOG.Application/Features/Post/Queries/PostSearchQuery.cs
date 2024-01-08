using AutoMapper;
using BLOG.Application.Caching;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Result;
using BLOG.Application.Wrappers;
using BLOG.Domain.Model.Post;
using BLOG.Domain.ReadModel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.Post.Queries
{
    public class PostSearchQuery : BaseCacheableQuery<Result<PagedList<PostSearchResult>>>
    {
        public override string CacheKey => $"{nameof(PostSearchQuery)}_{PageIndex}_{PageSize}";

        public override string CacheGroup => $"{nameof(Domain.Model.Post.Post)}";
    }

    public class PostSearchQueryHandler : IRequestHandler<PostSearchQuery, Result<PagedList<PostSearchResult>>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public PostSearchQueryHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
        }

        public async Task<Result<PagedList<PostSearchResult>>> Handle(PostSearchQuery request, CancellationToken cancellationToken)
        {
            if (request.PageIndex < 1)
            {
                return Result<PagedList<PostSearchResult>>.Invalid("Błędny indeks strony!");
            }

            if (request.PageSize < 1)
            {
                return Result<PagedList<PostSearchResult>>.Invalid("Błędna wielkość strony!");
            }

            var pager = new DataPager(request.PageIndex, request.PageSize);
            pager.TotalRecords = await _context.Posts.CountAsync(cancellationToken);
            pager.TotalPages = (int)Math.Ceiling((double)pager.TotalRecords / (double)request.PageSize);


            IQueryable<Domain.Model.Post.Post> query = _context.Posts.Include(x => x.User).OrderByDescending(x => x.PublishedAt);
            var pagedList = query.Skip(pager.PageSize * (pager.PageIndex - 1)).Take(pager.PageSize).ToList();

            var result = pagedList.Select(x => _mapper.Map<PostSearchResult>(x)).ToList();

            return Result<PagedList<PostSearchResult>>.Success(new PagedList<PostSearchResult>(pager, result));
        }
    }
}
