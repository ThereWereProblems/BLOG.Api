using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.Comment.Commands;
using BLOG.Application.Features.Comment.Events;
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

namespace BLOG.Application.UnitTests.Tests.Comment.Commands
{
    public class CommentUpdateCommandTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICurentUserService> _userService;
        private readonly Lazy<ICurentUserService> _lazyUserService;
        private readonly Mock<IMediator> _mediator;
        private readonly Domain.DTO.CreateCommentDTO dto;

        public CommentUpdateCommandTest()
        {
            _mapper = MapperMock.GetMapper();

            _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("48a1f000-7047-41ce-9b63-c9ba458487d5");

            _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            _mediator = new Mock<MediatR.IMediator>();
            _mediator.Setup(x => x.Publish(It.IsAny<CommentChangedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            dto = new Domain.DTO.CreateCommentDTO
            {
                Content = "NewContent"
            };
        }

        [Fact]
        public async Task CommentUpdateCommand_Owner_SuccessTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("48a1f000-7047-41ce-9b63-c9ba458487d5");
            _userService.Setup(x => x.IsAdmin).Returns(false);

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new CommentUpdateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new CommentUpdateCommand { Id = 1, CommentDTO = dto }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<bool>>();
            result.Value.ShouldBe(true);

            context.Comments.Count().ShouldBe(6);
            context.Comments.FirstOrDefault(x => x.Id == 1).Content.ShouldBe("NewContent");
        }

        [Fact]
        public async Task CommentUpdateCommand_Admin_SuccessTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("9baf84a0-f581-4606-afda-4537a8da5a7d");
            _userService.Setup(x => x.IsAdmin).Returns(true);

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new CommentUpdateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new CommentUpdateCommand { Id = 1, CommentDTO = dto }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<bool>>();
            result.Value.ShouldBe(true);

            context.Comments.Count().ShouldBe(6);
            context.Comments.FirstOrDefault(x => x.Id == 1).Content.ShouldBe("NewContent");
        }

        [Fact]
        public async Task CommentUpdateCommand_NoAccess_FailureTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("9baf84a0-f581-4606-afda-4537a8da5a7d");
            _userService.Setup(x => x.IsAdmin).Returns(false);

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new CommentUpdateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new CommentUpdateCommand { Id = 1, CommentDTO = dto }, CancellationToken.None);

            result.IsSuccess.ShouldBe(false);
            result.ShouldBeOfType<Result<bool>>();
            result.Problem.ShouldBe(AppProblems.Forbidden);

            context.Comments.Count().ShouldBe(6);
        }

        [Fact]
        public async Task CommentUpdateCommand_NotFound_FailureTest()
        {
            var _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("9baf84a0-f581-4606-afda-4537a8da5a7d");
            _userService.Setup(x => x.IsAdmin).Returns(true);

            var _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new CommentUpdateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new CommentUpdateCommand { Id = 7, CommentDTO = dto }, CancellationToken.None);

            result.IsSuccess.ShouldBe(false);
            result.ShouldBeOfType<Result<bool>>();
            result.Problem.ShouldBe(AppProblems.NotFound);

            context.Comments.Count().ShouldBe(6);
        }
    }
}
