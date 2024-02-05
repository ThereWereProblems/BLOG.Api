using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Result;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.File.Commands
{
    public class ImageDeleteCommand : IRequest<Result<bool>>
    {
        public string FileName { get; set; }
    }

    public class ImageDeleteCommandValidator : AbstractValidator<ImageDeleteCommand>
    {
        public ImageDeleteCommandValidator()
        {
            RuleFor(v => v.FileName)
                .NotNull().NotEmpty().WithMessage("Nazwa zdjęcia jest wymagana!");
        }
    }

    public class ImageDeleteCommandHandler : IRequestHandler<ImageDeleteCommand, Result<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public ImageDeleteCommandHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
        }

        public async Task<Result<bool>> Handle(ImageDeleteCommand request, CancellationToken cancellationToken)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", request.FileName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
          
            return Result<bool>.Success(true);

        }
    }
}
