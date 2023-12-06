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
    public class PostDeleteCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
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
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostDeleteCommandHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
            _contextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<Result<bool>> Handle(PostDeleteCommand request, CancellationToken cancellationToken)
        {
            var entry = _context.Posts.FirstOrDefault(x => x.Id == request.Id);

            if (entry == null)
                return Result<bool>.NotFound();

            _context.Posts.Remove(entry);
            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
