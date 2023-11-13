using BLOG.Application.Result;
using Correlate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BLOG.Api.Controllers
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected IActionResult HandleAppResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return result.Value is null ? base.NoContent() : base.Ok(result.Value);
            }
            return ResultForProblem(result.Problem, result.Errors);
        }

        private IActionResult ResultForProblem(AppProblem problem, IEnumerable<AppProblemDetail> errors)
        {
            var corId = HttpContext.RequestServices.GetService<ICorrelationContextAccessor>()
                ?.CorrelationContext?.CorrelationId;

            return problem.status switch
            {
                AppResultStatus.Forbidden => base.Forbid(),
                AppResultStatus.Unathorized => base.Unauthorized(),
                AppResultStatus.Invalid => base.BadRequest(MapToApiProblem(problem, errors, corId)),
                AppResultStatus.NotFound => base.NotFound(MapToApiProblem(problem, errors, corId)),
                AppResultStatus.Conflict => base.Conflict(MapToApiProblem(problem, errors, corId)),
                AppResultStatus.Error => new ObjectResult(MapToApiProblem(problem, errors, corId))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                },
                _ => throw new NotImplementedException($"ResultForProblem cannot handle AppResultStatus.{problem.status}")
            };
        }

        public static ApiProblem MapToApiProblem(AppProblem problem, IEnumerable<AppProblemDetail> errors, string corId = null)
        {
            var mapped = new ApiProblem
            {
                Status = problem.status switch
                {
                    AppResultStatus.Error => StatusCodes.Status500InternalServerError,
                    AppResultStatus.Forbidden => StatusCodes.Status403Forbidden,
                    AppResultStatus.Unathorized => StatusCodes.Status401Unauthorized,
                    AppResultStatus.Invalid => StatusCodes.Status400BadRequest,
                    AppResultStatus.NotFound => StatusCodes.Status404NotFound,
                    AppResultStatus.Conflict => StatusCodes.Status409Conflict,
                    _ => throw new NotImplementedException($"GetHTTPStatus cannot handle AppResultStatus.{problem.status}")
                },
                Title = problem.title,
            };

            if(errors is not null && errors.Any())
            {
                var apiErrors = errors
                    .Select(x => new ApiErrorProblem
                    {
                        Message = x.ErrorMessage,
                    }).ToList();
                mapped.Errors = apiErrors;
            }
            
            if(corId is not null)
            {
                mapped.CorrelationId = corId;
            }

            return mapped;
        }

        public class ApiProblem : ProblemDetails
        {
            [System.Text.Json.Serialization.JsonPropertyName("correlationId")]
            public string CorrelationId { get; set; }
            [System.Text.Json.Serialization.JsonPropertyName("errors")]
            public IEnumerable<ApiErrorProblem> Errors { get; set; }
        }

        public class ApiErrorProblem
        {
            [System.Text.Json.Serialization.JsonPropertyName("message")]
            public string Message { get; set; }
        }
    }
}
