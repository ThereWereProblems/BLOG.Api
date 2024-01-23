using AutoMapper;
using BLOG.Domain.DTO;
using BLOG.Domain.Model.ApplicationUser;
using BLOG.Domain.ReadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLOG.Application.AutoMapper
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<RegisterAppUserDTO, ApplicationUser>();
            CreateMap<ApplicationUser, UserInfoResult>();
        }
    }
}
