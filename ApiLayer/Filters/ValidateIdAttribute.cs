using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static DVLDApi.Helpers.ApiResponse;

namespace ApiLayer.Filters
{
    public class ValidateIdAttribute : ActionFilterAttribute
    {
        private readonly string[] _parameterNames;
        public ValidateIdAttribute(params string[] parameterNames)
        {
            _parameterNames = parameterNames;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var parameter in _parameterNames)
            {
                if (context.ActionArguments.TryGetValue(parameter, out var value))
                {
                    if (value is int id && id < 1)
                    {
                        context.Result = new BadRequestObjectResult(CreateResponse(StatusFail, $"The Provided {parameter} '{id}' is inValid. {parameter} must be greater than 0."));
                    }
                }
            }
        }
    }
}
