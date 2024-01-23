using BLOG.Application.Common.Abstractions;
using BLOG.Domain.Model.ApplicationUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Infrastructure.Services
{
    public class CurentUserService : ICurentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CurentUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager) 
        {
            _contextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string UserId => GetUserId();

        private string? GetUserId()
        {
            return GetUserIdAsync().Result;
        }
        private async Task<string?> GetUserIdAsync()
        {
            if(_contextAccessor.HttpContext == null) //ApplicationDbContextSeed
                return null;

            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);

            if (user is null)
                return null;

            return user?.Id;
        }
    }
}
