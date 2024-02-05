using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.Post.Commands;
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
    public class ImageCreateCommand : IRequest<Result<string>>
    {
        public IFormFile File { get; set; }
    }

    public class ImageCreateCommandValidator : AbstractValidator<ImageCreateCommand>
    {
        public ImageCreateCommandValidator()
        {
            RuleFor(v => v.File)
                .NotNull().WithMessage("Zdjęcie jest wymagane");
        }
    }

    public class ImageCreateCommandHandler : IRequestHandler<ImageCreateCommand, Result<string>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly ICurentUserService _userService;

        public ImageCreateCommandHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext, ICurentUserService curentUserService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
            _userService = curentUserService;
        }

        public async Task<Result<string>> Handle(ImageCreateCommand request, CancellationToken cancellationToken)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);
            if (request.File != null && request.File.Length > 0)
            {
                if (!System.IO.Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images")))
                    System.IO.Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images"));
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(fileStream);
                }
                return Result<string>.Success(fileName);
            }
            return Result<string>.Invalid("Nieprawidłowy format zdjęcia!");
        }
    }
}
