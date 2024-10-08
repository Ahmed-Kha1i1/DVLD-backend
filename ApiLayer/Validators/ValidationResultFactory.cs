using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using static DVLDApi.Helpers.ApiResponse;
namespace ApiLayer.Validators
{
    public class ValidationResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            return new BadRequestObjectResult(CreateResponse(StatusError, "One or more Valodation error occurred.", validationProblemDetails?.Errors));
        }
    }
}

