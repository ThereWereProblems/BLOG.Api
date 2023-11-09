using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.WeatherForecast.Queries
{
    public class WeatherForecastGetQuery : IRequest<List<Domain.Model.WeatherForecast.WeatherForecast>>
    {

    }

    public class WeatherForecastGetQueryValidator : AbstractValidator<WeatherForecastGetQuery>
    {
        public WeatherForecastGetQueryValidator()
        {

        }
    }

    public class WeatherForecastGetQueryHandler : IRequestHandler<WeatherForecastGetQuery, List<Domain.Model.WeatherForecast.WeatherForecast>>
    {
        private readonly IMediator _mediator;

        public WeatherForecastGetQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<Domain.Model.WeatherForecast.WeatherForecast>> Handle(WeatherForecastGetQuery request, CancellationToken cancellationToken)
        {
            var Summaries = new[]
            {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            return Enumerable.Range(1, 5).Select(index => new Domain.Model.WeatherForecast.WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToList();
        }
    }
}
