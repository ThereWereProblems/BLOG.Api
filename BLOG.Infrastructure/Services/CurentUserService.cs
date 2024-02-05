using BLOG.Application.Common.Abstractions;
using BLOG.Domain.Model.ApplicationUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public ClaimsPrincipal ClaimsPrincipal => GetClaimsPrincipal();

        public bool IsAdmin => CheckIfIsAdmin();

        public List<string> Roles => GetRoles();



        private string? GetUserId()
        {
            return GetUserIdAsync().Result;
        }

        private async Task<string?> GetUserIdAsync()
        {
            if (_contextAccessor.HttpContext == null) //ApplicationDbContextSeed
                return null;

            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);

            if (user is null)
                return null;

            return user?.Id;
        }

        private ClaimsPrincipal? GetClaimsPrincipal()
        {
            return GetClaimsPrincipalAsync().Result;
        }

        private async Task<ClaimsPrincipal?> GetClaimsPrincipalAsync()
        {
            if (_contextAccessor.HttpContext == null) //ApplicationDbContextSeed
                return null;

            return _contextAccessor.HttpContext.User;
        }

        private bool CheckIfIsAdmin()
        {
            return CheckIfIsAdminAsync().Result;
        }

        private async Task<bool> CheckIfIsAdminAsync()
        {
            var roles = Roles;

            if (roles.Contains("Admin"))
                return true;

            return false;
        }

        private List<string> GetRoles()
        {
            return GetRolesAsync().Result;
        }

        private async Task<List<string>> GetRolesAsync()
        {
            var user = ClaimsPrincipal;
            var userIdentity = (ClaimsIdentity)user.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

            return roles;
        }
    }
}
