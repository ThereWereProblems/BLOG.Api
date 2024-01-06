using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.Post.Commands;
using BLOG.Application.Features.Post.Queries;
using BLOG.Application.Result;
using BLOG.Domain.ReadModel;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BLOG.Application.Features.Post.Queries
{
    public class PostGetImageQuery : IRequest<Result<FileStream>>
    {
        public string FileName { get; set; }
    }

    public class PostGetImageQueryValidator : AbstractValidator<PostGetImageQuery>
    {
        public PostGetImageQueryValidator()
        {
            RuleFor(v => v.FileName)
                .NotNull()
                .NotEmpty().WithMessage("Nazwa jest wymagana!");
        }
    }

    public class PostGetImageQueryHandler : IRequestHandler<PostGetImageQuery, Result<FileStream>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public PostGetImageQueryHandler(IMediator mediator, IMapper mapper, IApplicationDbContext appDbContext)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = appDbContext;
        }

        public async Task<Result<FileStream>> Handle(PostGetImageQuery request, CancellationToken cancellationToken)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", request.FileName);
            
            if (!System.IO.File.Exists(path))
                return Result<FileStream>.NotFound();

            var image = System.IO.File.OpenRead(path);

            return Result<FileStream>.Success(image);

            //string _base64String = null;

            //using (System.Drawing.Image _image = System.Drawing.Image.FromFile(path))
            //{
            //    using (MemoryStream _mStream = new MemoryStream())
            //    {
            //        _image.Save(_mStream, _image.RawFormat);
            //        byte[] _imageBytes = _mStream.ToArray();
            //        _base64String = Convert.ToBase64String(_imageBytes);

            //        var result = "data:image/" + Path.GetExtension(path).Trim('.') + ";base64," + _base64String;
            //        return Result<string>.Success(_base64String);
            //    }
            //}
        }
    }
}
