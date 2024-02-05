using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.File.Commands;
using BLOG.Application.Features.Post.Commands;
using BLOG.Application.Features.Post.Queries;
using BLOG.Application.Result;
using BLOG.Application.UnitTests.Mocks;
using BLOG.Domain.ReadModel;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.UnitTests.Tests.Post.Commands
{
    public class PostCreateCommandTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICurentUserService> _userService;
        private readonly Lazy<ICurentUserService> _lazyUserService;

        public PostCreateCommandTest()
        {
            _mapper = MapperMock.GetMapper();

            _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("48a1f000-7047-41ce-9b63-c9ba458487d5");

            _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);
        }

        [Fact]
        public async Task PostCreateCommand_SuccessTest()
        {
            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var _mediator = new Mock<MediatR.IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<ImageCreateCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<string>.Success("FileName"));

            var handler = new PostCreateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var dto = new Domain.DTO.CreatePostDTO
            {
                Title = "Title",
                Description = "Description",
                Content = "Content"
            };

            var result = await handler.Handle(new PostCreateCommand { PostDTO =  dto}, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<int>>();
            result.Value.ShouldBe(4);

            context.Posts.Count().ShouldBe(4);
            context.Posts.FirstOrDefault(x => x.Id == 4).Title.ShouldBe("Title");
            context.Posts.FirstOrDefault(x => x.Id == 4).Description.ShouldBe("Description");
            context.Posts.FirstOrDefault(x => x.Id == 4).Content.ShouldBe("Content");
            context.Posts.FirstOrDefault(x => x.Id == 4).Image.ShouldBe("FileName");
        }

        [Fact]
        public async Task PostCreateCommand_FailureTest()
        {
            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var _mediator = new Mock<MediatR.IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<ImageCreateCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<string>.Invalid("Błąd podczas zapisywania zdjęcia!"));

            var handler = new PostCreateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var dto = new Domain.DTO.CreatePostDTO
            {
                Title = "Title",
                Description = "Description",
                Content = "Content"
            };

            var result = await handler.Handle(new PostCreateCommand { PostDTO = dto }, CancellationToken.None);

            result.IsSuccess.ShouldBe(false);
            result.ShouldBeOfType<Result<int>>();
            result.Errors.First().ErrorMessage.ShouldBe("Błąd podczas zapisywania zdjęcia!");

            context.Posts.Count().ShouldBe(3);
        }
    }
}
