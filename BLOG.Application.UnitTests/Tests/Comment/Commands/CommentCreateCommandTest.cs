using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.Comment.Commands;
using BLOG.Application.Features.Comment.Events;
using BLOG.Application.Features.File.Commands;
using BLOG.Application.Result;
using BLOG.Application.UnitTests.Mocks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.UnitTests.Tests.Comment.Commands
{
    public class CommentCreateCommandTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICurentUserService> _userService;
        private readonly Lazy<ICurentUserService> _lazyUserService;
        private readonly Mock<IMediator> _mediator;

        public CommentCreateCommandTest() 
        {
            _mapper = MapperMock.GetMapper();

            _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("48a1f000-7047-41ce-9b63-c9ba458487d5");

            _lazyUserService = new Lazy<ICurentUserService>(() => _userService.Object);

            _mediator = new Mock<MediatR.IMediator>();
            _mediator.Setup(x => x.Publish(It.IsAny<CommentChangedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task CommentCreateCommand_SuccessTest()
        {
            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new CommentCreateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var dto = new Domain.DTO.CreateCommentDTO
            {
                PostId = 1,
                Content = "Content"
            };

            var result = await handler.Handle(new CommentCreateCommand { CommentDTO = dto }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<int>>();
            result.Value.ShouldBe(7);

            context.Comments.Count().ShouldBe(7);
            context.Comments.Where(x => x.PostId == 1).Count().ShouldBe(3);
            context.Comments.FirstOrDefault(x => x.Id == 7).Content.ShouldBe("Content");
        }

        [Fact]
        public async Task CommentCreateCommand_FailureTest()
        {
            using var context = new ApplicationDbContextMock().CreateContext(_lazyUserService);

            var handler = new CommentCreateCommandHandler(_mediator.Object, _mapper, context, _userService.Object);

            var dto = new Domain.DTO.CreateCommentDTO
            {
                PostId = 4,
                Content = "Content"
            };

            var result = await handler.Handle(new CommentCreateCommand { CommentDTO = dto }, CancellationToken.None);

            result.IsSuccess.ShouldBe(false);
            result.ShouldBeOfType<Result<int>>();
            result.Errors.First().ErrorMessage.ShouldBe("Post o podanym Id nie istnieje!");

            context.Comments.Count().ShouldBe(6);
        }
    }
}
