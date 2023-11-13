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

        //private readonly ILogger<WeatherForecastController> _logger;

        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
        //{
        //    _logger = logger;
        //}

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}
