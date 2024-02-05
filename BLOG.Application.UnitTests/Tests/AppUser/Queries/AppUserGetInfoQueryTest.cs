using AutoMapper;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.AppUser.Queries;
using BLOG.Application.Features.Comment.Queries;
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

namespace BLOG.Application.UnitTests.Tests.AppUser.Queries
{
    public class AppUserGetInfoQueryTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICurentUserService> _userService;
        private readonly Mock<IMediator> _mediator;

        public AppUserGetInfoQueryTest()
        {
            _mapper = MapperMock.GetMapper();

            _mediator = new Mock<MediatR.IMediator>();

            _userService = new Mock<ICurentUserService>();
            _userService.Setup(x => x.UserId).Returns("48a1f000-7047-41ce-9b63-c9ba458487d5");
            _userService.Setup(x => x.Roles).Returns(new List<string> { "Admin", "Member" });
        }

        [Fact]
        public async Task AppUserGetInfoQuery_SuccessTest()
        {
            using var context = new ApplicationDbContextMock().CreateContext();

            var handler = new AppUserGetInfoQueryHandler(_mediator.Object, _mapper, context, _userService.Object);

            var result = await handler.Handle(new AppUserGetInfoQuery(), CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.ShouldBeOfType<Result<UserInfoResult>>();
            result.Value.NickName.ShouldBe("Dawid");
            result.Value.Email.ShouldBe("user@example.com");
            result.Value.Roles.Count().ShouldBe(2);
            result.Value.Roles.Contains("Admin").ShouldBe(true);
            result.Value.Roles.Contains("Member").ShouldBe(true);
        }
    }
}
