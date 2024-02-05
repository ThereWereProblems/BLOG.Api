using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.File.Commands;
using BLOG.Application.Features.Post.Commands;
using BLOG.Application.Result;
using BLOG.Application.UnitTests.Mocks;
using MediatR;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.UnitTests.Tests.Post.Commands
{
    public class PostDeleteCommandTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IMediator> _mediator;

        public PostDeleteCommandTest()
        {
            _mapper = MapperMock.GetMapper();

            _mediator = new Mock<MediatR.IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<ImageDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<bool>.Success(true));
        }

        [Fact]
        public async Task PostDeleteCommand_Owner_SuccessTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("48a1f000-7047-41ce-9b63-c9ba458487d5");
            _userService.Setup(x => x.IsAdmin).Returns(false);

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new PostDeleteCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new PostDeleteCommand { Id = 1 }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<bool>>();
            result.Value.ShouldBe(true);

            context.Posts.Count().ShouldBe(2);
            context.Posts.FirstOrDefault(x => x.Id == 1).ShouldBe(null);
            context.Comments.Where(x => x.PostId == 1).Count().ShouldBe(0);
        }

        [Fact]
        public async Task PostDeleteCommand_Admin_SuccessTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("9baf84a0-f581-4606-afda-4537a8da5a7d");
            _userService.Setup(x => x.IsAdmin).Returns(true);

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new PostDeleteCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new PostDeleteCommand { Id = 1 }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<bool>>();
            result.Value.ShouldBe(true);

            context.Posts.Count().ShouldBe(2);
            context.Posts.FirstOrDefault(x => x.Id == 1).ShouldBe(null);
            context.Comments.Where(x => x.PostId == 1).Count().ShouldBe(0);
        }

        [Fact]
        public async Task PostDeleteCommand_NoAccess_FailureTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("9baf84a0-f581-4606-afda-4537a8da5a7d");
            _userService.Setup(x => x.IsAdmin).Returns(false);

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new PostDeleteCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new PostDeleteCommand { Id = 1 }, CancellationToken.None);

            result.IsSuccess.ShouldBe(false);
            result.ShouldBeOfType<Result<bool>>();
            result.Problem.ShouldBe(AppProblems.Forbidden);

            context.Posts.Count().ShouldBe(3);
        }

        [Fact]
        public async Task PostDeleteCommand_NotFound_FailureTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("9baf84a0-f581-4606-afda-4537a8da5a7d");
            _userService.Setup(x => x.IsAdmin).Returns(true);

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new PostDeleteCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new PostDeleteCommand { Id = 4 }, CancellationToken.None);

            result.IsSuccess.ShouldBe(false);
            result.ShouldBeOfType<Result<bool>>();
            result.Problem.ShouldBe(AppProblems.NotFound);

            context.Posts.Count().ShouldBe(3);
        }
    }
}
