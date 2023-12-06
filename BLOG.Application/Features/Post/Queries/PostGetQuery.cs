using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.Post.Commands;
using BLOG.Application.Result;
using BLOG.Domain.DTO;
using BLOG.Domain.Model.ApplicationUser;
using BLOG.Domain.ReadModel;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.Post.Queries
{
    public class PostGetQuery : IRequest<Result<PostSearchResult>>
    {
        public int Id { get; set; }
    }

    

    public class PostGetQueryHandler : IRequestHandler<PostGetQuery, Result<PostSearchResult>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public PostGetQueryHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
        }

        public async Task<Result<PostSearchResult>> Handle(PostGetQuery request, CancellationToken cancellationToken)
        {
            var entry = _context.Posts.Include(v => v.User).FirstOrDefault(x => x.Id == request.Id);

            if (entry == null)
            {
                return Result<PostSearchResult>.NotFound();
            }

            var result = _mapper.Map<Domain.ReadModel.PostSearchResult>(entry);

            return Result<PostSearchResult>.Success(result);
        }
    }
}
