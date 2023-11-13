using BLOG.Domain.Model.WeatherForecast;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLOG.Application.Features.WeatherForecast.Queries;
using Microsoft.AspNetCore.Identity.Data;
using BLOG.Domain.DTO;
using BLOG.Application.Features.AppUser.Commands;

namespace BLOG.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ApiControllerBase
    {
        /// <summary>
        /// Rejestruj użytkownika
        /// </summary>
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
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies)
        {
            return HandleAppResult( await Mediator.Send(new AppUserLoginCommand { Login = dto, useCookies = useCookies, useSessionCookies = useSessionCookies }));
        }
    }
}
