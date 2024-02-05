using AutoMapper;
using BLOG.Application.Features.Post.Queries;
using BLOG.Application.Result;
using BLOG.Application.UnitTests.Mocks;
using BLOG.Application.Wrappers;
using BLOG.Domain.ReadModel;
using MediatR;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.UnitTests.Tests.Post.Queries
{
    public class PostGetQueryTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IMediator> _mediator;

        public PostGetQueryTest()
        {
            _mapper = MapperMock.GetMapper();

            _mediator = new Mock<MediatR.IMediator>();
        }

        [Fact]
        public async Task PostGetQuery_SuccessTest()
        {
            using var context = new ApplicationDbContextMock().CreateContext();

            var handler = new PostGetQueryHandler(_mediator.Object, _mapper, context);

            var result = await handler.Handle(new PostGetQuery { Id = 2 }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<PostDetailResult>>();
            result.Value.ShouldNotBeNull();
            result.Value.Id.ShouldBe(2);
            result.Value.Author.ShouldBe("dawid002");
            result.Value.PublishedAt.ShouldBeOfType<DateTime>();
            result.Value.Title.ShouldNotBeNullOrEmpty();
            result.Value.Description.ShouldNotBeNullOrEmpty();
            result.Value.Content.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task PostGetQuery_FailureTest()
        {
            using var context = new ApplicationDbContextMock().CreateContext();

            var handler = new PostGetQueryHandler(_mediator.Object, _mapper, context);

            var result = await handler.Handle(new PostGetQuery { Id = 4 }, CancellationToken.None);

            result.IsSuccess.ShouldBe(false);
            result.ShouldBeOfType<Result<PostDetailResult>>();
            result.Problem.ShouldBe(AppProblems.NotFound);
        }
    }
}
