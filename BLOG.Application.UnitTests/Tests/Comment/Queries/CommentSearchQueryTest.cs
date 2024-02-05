using AutoMapper;
using BLOG.Application.Features.Comment.Queries;
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

namespace BLOG.Application.UnitTests.Tests.Comment.Queries
{
    public class CommentSearchQueryTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IMediator> _mediator;

        public CommentSearchQueryTest()
        {
            _mapper = MapperMock.GetMapper();

            _mediator = new Mock<MediatR.IMediator>();
        }

        [Fact]
        public async Task CommentSearchQuery_OnePageTest()
        {
            using var context = new ApplicationDbContextMock().CreateContext();

            var handler = new CommentSearchQueryHandler(_mediator.Object, _mapper, context);

            var result = await handler.Handle(new CommentSearchQuery { PostId = 2 }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<PagedList<CommentSearchResult>>>();
            result.Value.DataPager.PageSize.ShouldBe(20);
            result.Value.DataPager.PageIndex.ShouldBe(1);
            result.Value.DataPager.TotalPages.ShouldBe(1);
            result.Value.Result.Count.ShouldBe(2);
        }

        [Fact]
        public async Task CommentSearchQuery_TwoPagesTest()
        {
            using var context = new ApplicationDbContextMock().CreateContext();

            var handler = new CommentSearchQueryHandler(_mediator.Object, _mapper, context);

            var result = await handler.Handle(new CommentSearchQuery { PostId = 2, PageIndex = 2, PageSize = 1 }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<PagedList<CommentSearchResult>>>();
            result.Value.DataPager.PageSize.ShouldBe(1);
            result.Value.DataPager.PageIndex.ShouldBe(2);
            result.Value.DataPager.TotalPages.ShouldBe(2);
            result.Value.Result.Count.ShouldBe(1);
        }
    }
}
