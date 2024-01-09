using AutoMapper;
using BLOG.Domain.DTO;
using BLOG.Domain.Model.Comment;
using BLOG.Domain.ReadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.AutoMapper
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CreateCommentDTO, Domain.Model.Comment.Comment>();
            CreateMap<Domain.Model.Comment.Comment, CommentSearchResult>().ForMember(x => x.User, v => v.MapFrom(s => s.User.NickName));
        }
    }
}
