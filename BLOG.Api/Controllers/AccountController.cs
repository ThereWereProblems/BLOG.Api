using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.Data;
using BLOG.Domain.DTO;
using BLOG.Application.Features.AppUser.Commands;
using BLOG.Domain.ReadModel;
using BLOG.Application.Features.AppUser.Queries;

namespace BLOG.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ApiControllerBase
    {
        /// <summary>
        /// Rejestruj użytkownika
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> Register([FromBody] RegisterAppUserDTO dto)
        {
            return HandleAppResult( await Mediator.Send(new AppUserRegisterCommand { UserDTO = dto }));
        }

        /// <summary>
        /// Zaloguj się
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="useCookies"></param>
        /// <param name="useSessionCookies"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies)
        {
            return HandleAppResult( await Mediator.Send(new AppUserLoginCommand { Login = dto, useCookies = useCookies, useSessionCookies = useSessionCookies }));
        }

        /// <summary>
        /// Odśwież token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest dto)
        {
            return HandleAppResult(await Mediator.Send(new AppUserRefreshCommand { Model = dto }));
        }

        /// <summary>
        /// Pobierz informacje o użytkowniku
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("userinfo")]
        [ProducesResponseType(typeof(UserInfoResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserInfo()
        {
            return HandleAppResult(await Mediator.Send(new AppUserGetInfoQuery()));
        }
    }
}
