using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Common.Behaviours
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehavior(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMiliseconds = _timer.ElapsedMilliseconds;

            if(elapsedMiliseconds > 2000)
            {
                _logger.LogWarning("Behaviour Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                    typeof(TRequest).Name, elapsedMiliseconds, request);
            }

            return response;
        }
    }
}
