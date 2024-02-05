using AutoMapper;
using BLOG.Application.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.UnitTests.Mocks
{
    public static class MapperMock
    {
        public static IMapper GetMapper()
        {
            var maperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<AppUserProfile>();
                c.AddProfile<CommentProfile>();
                c.AddProfile<PostProfile>();
            });

            return maperConfig.CreateMapper();
        }
    }
}
