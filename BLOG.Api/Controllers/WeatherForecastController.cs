using BLOG.Domain.Model.WeatherForecast;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLOG.Application.Features.WeatherForecast.Queries;

namespace BLOG.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class WeatherForecastController : ApiControllerBase
    {
        /// <summary>
        /// Generuj pogodê
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<WeatherForecast>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return HandleAppResult(await Mediator.Send(new WeatherForecastGetQuery()));
        }
    }
}
