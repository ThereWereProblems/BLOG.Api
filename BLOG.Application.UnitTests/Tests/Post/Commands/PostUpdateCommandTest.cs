using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.File.Commands;
using BLOG.Application.Features.Post.Commands;
using BLOG.Application.Result;
using BLOG.Application.UnitTests.Mocks;
using BLOG.Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.UnitTests.Tests.Post.Commands
{
    public class PostUpdateCommandTest
    {
        private readonly IMapper _mapper;
        private readonly CreatePostDTO dto;
        private readonly IFormFile file;

        public PostUpdateCommandTest()
        {
            _mapper = MapperMock.GetMapper();

            dto = new Domain.DTO.CreatePostDTO
            {
                Title = "Title",
                Description = "Description",
                Content = "Content"
            };

            var stream = new MemoryStream();
            file = new FormFile(stream, 0, stream.Length, "id_from_form", "fileName");
        }

        [Fact]
        public async Task PostUpdateCommand_Owner_WithoutFile_SuccessTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("48a1f000-7047-41ce-9b63-c9ba458487d5");
            _userService.Setup(x => x.IsAdmin).Returns(false);

            var _mediator = new Mock<MediatR.IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<ImageDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<bool>.Success(true));

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new PostUpdateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new PostUpdateCommand { Id = 1, PostDTO = dto }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<bool>>();
            result.Value.ShouldBe(true);

            context.Posts.Count().ShouldBe(3);
            context.Posts.FirstOrDefault(x => x.Id == 1).Title.ShouldBe("Title");
            context.Posts.FirstOrDefault(x => x.Id == 1).Description.ShouldBe("Description");
            context.Posts.FirstOrDefault(x => x.Id == 1).Content.ShouldBe("Content");
            context.Posts.FirstOrDefault(x => x.Id == 1).Image.ShouldBe("9901d690-59e0-4991-a460-b0a388ce5a20.jpg");
        }

        [Fact]
        public async Task PostUpdateCommand_Admin_WithoutFile_SuccessTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("9baf84a0-f581-4606-afda-4537a8da5a7d");
            _userService.Setup(x => x.IsAdmin).Returns(true);

            var _mediator = new Mock<MediatR.IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<ImageDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<bool>.Success(true));

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new PostUpdateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new PostUpdateCommand { Id = 1, PostDTO = dto }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<bool>>();
            result.Value.ShouldBe(true);

            context.Posts.Count().ShouldBe(3);
            context.Posts.FirstOrDefault(x => x.Id == 1).Title.ShouldBe("Title");
            context.Posts.FirstOrDefault(x => x.Id == 1).Description.ShouldBe("Description");
            context.Posts.FirstOrDefault(x => x.Id == 1).Content.ShouldBe("Content");
            context.Posts.FirstOrDefault(x => x.Id == 1).Image.ShouldBe("9901d690-59e0-4991-a460-b0a388ce5a20.jpg");
        }

        [Fact]
        public async Task PostUpdateCommand_Owner_WithFile_SuccessTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("48a1f000-7047-41ce-9b63-c9ba458487d5");
            _userService.Setup(x => x.IsAdmin).Returns(false);

            var _mediator = new Mock<MediatR.IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<ImageDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<bool>.Success(true));
            _mediator.Setup(x => x.Send(It.IsAny<ImageCreateCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<string>.Success("FileName"));

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new PostUpdateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new PostUpdateCommand { Id = 1, PostDTO = dto, File = file }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<bool>>();
            result.Value.ShouldBe(true);

            context.Posts.Count().ShouldBe(3);
            context.Posts.FirstOrDefault(x => x.Id == 1).Title.ShouldBe("Title");
            context.Posts.FirstOrDefault(x => x.Id == 1).Description.ShouldBe("Description");
            context.Posts.FirstOrDefault(x => x.Id == 1).Content.ShouldBe("Content");
            context.Posts.FirstOrDefault(x => x.Id == 1).Image.ShouldBe("FileName");
        }

        [Fact]
        public async Task PostUpdateCommand_Admin_WithFile_SuccessTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("9baf84a0-f581-4606-afda-4537a8da5a7d");
            _userService.Setup(x => x.IsAdmin).Returns(true);

            var _mediator = new Mock<MediatR.IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<ImageDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<bool>.Success(true));
            _mediator.Setup(x => x.Send(It.IsAny<ImageCreateCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<string>.Success("FileName"));

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new PostUpdateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new PostUpdateCommand { Id = 1, PostDTO = dto, File = file }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<bool>>();
            result.Value.ShouldBe(true);

            context.Posts.Count().ShouldBe(3);
            context.Posts.FirstOrDefault(x => x.Id == 1).Title.ShouldBe("Title");
            context.Posts.FirstOrDefault(x => x.Id == 1).Description.ShouldBe("Description");
            context.Posts.FirstOrDefault(x => x.Id == 1).Content.ShouldBe("Content");
            context.Posts.FirstOrDefault(x => x.Id == 1).Image.ShouldBe("FileName");
        }

        [Fact]
        public async Task PostUpdateCommand_NoAccess_FailureTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("9baf84a0-f581-4606-afda-4537a8da5a7d");
            _userService.Setup(x => x.IsAdmin).Returns(false);

            var _mediator = new Mock<MediatR.IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<ImageDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<bool>.Success(true));

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new PostUpdateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new PostUpdateCommand { Id = 1, PostDTO = dto }, CancellationToken.None);

            result.IsSuccess.ShouldBe(false);
            result.ShouldBeOfType<Result<bool>>();
            result.Problem.ShouldBe(AppProblems.Forbidden);

            context.Posts.Count().ShouldBe(3);
        }

        [Fact]
        public async Task PostUpdateCommand_NotFound_FailureTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("9baf84a0-f581-4606-afda-4537a8da5a7d");
            _userService.Setup(x => x.IsAdmin).Returns(true);

            var _mediator = new Mock<MediatR.IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<ImageDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<bool>.Success(true));

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new PostUpdateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new PostUpdateCommand { Id = 4, PostDTO = dto }, CancellationToken.None);

            result.IsSuccess.ShouldBe(false);
            result.ShouldBeOfType<Result<bool>>();
            result.Problem.ShouldBe(AppProblems.NotFound);

            context.Posts.Count().ShouldBe(3);
        }

        [Fact]
        public async Task PostUpdateCommand_ImageSaveError_FailureTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("48a1f000-7047-41ce-9b63-c9ba458487d5");
            _userService.Setup(x => x.IsAdmin).Returns(true);

            var _mediator = new Mock<MediatR.IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<ImageDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<bool>.Success(true));
            _mediator.Setup(x => x.Send(It.IsAny<ImageCreateCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<string>.Invalid("Błąd podczas zapisywania zdjęcia!"));

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new PostUpdateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new PostUpdateCommand { Id = 1, PostDTO = dto, File = file }, CancellationToken.None);

            result.IsSuccess.ShouldBe(false);
            result.ShouldBeOfType<Result<bool>>();
            result.Problem.ShouldBe(AppProblems.Error);
            result.Errors.First().ErrorMessage.ShouldBe("Błąd podczas zapisywania zdjęcia!");

            context.Posts.Count().ShouldBe(3);
        }
    }
}
