using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Result
{
    public class Result<T> : IAppResult
    {
        public Result(T value)
        {
            Value = value;
            if (value == null)
            {
                ValueType = Value.GetType();
            }
        }

        private Result(AppProblem appProblem)
        {
            Problem = appProblem;
        }

        private Result(AppProblem appProblem, AppProblemDetail appProblemDetail)
        {
            Problem = appProblem;
            Errors = new[] { appProblemDetail };
        }

        public static implicit operator T?(Result<T> result) => result.Value;
        public static implicit operator Result<T>(T value) => Success(value);

        public T? Value { get; set; }
        public Type? ValueType { get; set; }
        public AppProblem? Problem { get; private set; } = null;
        public bool IsSuccess => Problem == null;
        public string SuccessMessage { get; private set; } = string.Empty;
        public IEnumerable<AppProblemDetail> Errors { get; private set; } = new List<AppProblemDetail>();
        public void ClearValueType() => ValueType = null;
        public object? GetValue() => Value;

        public static Result<T> Success(T value) => new(value);
        public static Result<T> Success(T value, string successMessage) => new(value) { SuccessMessage = successMessage };
        public static Result<T> Error(string simpleErrorDetail) => new(AppProblems.Error, new AppProblemDetail("", simpleErrorDetail));
        public static Result<T> Error(AppProblemDetail error) => new(AppProblems.Error, error);
        public static Result<T> Invalid(List<AppProblemDetail> errors) => new(AppProblems.Invalid) { Errors = errors };
        public static Result<T> Invalid(string message) => new(AppProblems.Invalid, new AppProblemDetail("", message));
        public static Result<T> NotFound() => new(AppProblems.NotFound);
        public static Result<T> Forbidden() => new(AppProblems.Forbidden);
        public static Result<T> Unauthorized() => new(AppProblems.Unathorized);
    }
}
