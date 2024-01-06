using AutoMapper;
using BLOG.Domain.DTO;
using BLOG.Domain.ReadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.AutoMapper
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<CreatePostDTO, Domain.Model.Post.Post>();
            CreateMap<Domain.Model.Post.Post, PostSearchResult>().ForMember(x => x.Author, v => v.MapFrom(s => s.User.NickName));
            CreateMap<Domain.Model.Post.Post, PostDetailResult>().ForMember(x => x.Author, v => v.MapFrom(s => s.User.NickName));
        }
    }
}
