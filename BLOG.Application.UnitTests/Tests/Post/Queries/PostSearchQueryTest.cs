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
    public class PostSearchQueryTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IMediator> _mediator;

        public PostSearchQueryTest()
        {
            _mapper = MapperMock.GetMapper();
            _mediator = new Mock<MediatR.IMediator>();
        }

        [Fact]
        public async Task PostSearchQuery_OnePageTest()
        {
            using var context = new ApplicationDbContextMock().CreateContext();

            var handler = new PostSearchQueryHandler(_mediator.Object, _mapper, context);

            var result = await handler.Handle(new PostSearchQuery(), CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<PagedList<PostSearchResult>>>();
            result.Value.DataPager.PageSize.ShouldBe(20);
            result.Value.DataPager.PageIndex.ShouldBe(1);
            result.Value.DataPager.TotalPages.ShouldBe(1);
            result.Value.Result.Count.ShouldBe(3);
        }

        [Fact]
        public async Task PostSearchQuery_TwoPagesTest()
        {
            using var context = new ApplicationDbContextMock().CreateContext();

            var handler = new PostSearchQueryHandler(_mediator.Object, _mapper, context);

            var result = await handler.Handle(new PostSearchQuery { PageIndex = 2, PageSize = 2 }, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<PagedList<PostSearchResult>>>();
            result.Value.DataPager.PageSize.ShouldBe(2);
            result.Value.DataPager.PageIndex.ShouldBe(2);
            result.Value.DataPager.TotalPages.ShouldBe(2);
            result.Value.Result.Count.ShouldBe(1);
        }
    }
}
