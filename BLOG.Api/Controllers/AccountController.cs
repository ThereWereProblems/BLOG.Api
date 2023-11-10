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
    public class AccountController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        /// <summary>
        /// Rejestruj użytkownika
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> Register([FromBody] RegisterAppUserDTO dto)
        {
            var result = await Mediator.Send(new AppUserRegisterCommand { UserDTO = dto });

            if (result)
                return Accepted();
            else
                return BadRequest();
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
            var result = await Mediator.Send(new AppUserLoginCommand { Login = dto, useCookies = useCookies, useSessionCookies = useSessionCookies });

            if (result)
                return Accepted();
            else
                return BadRequest();
        }
    }
}
