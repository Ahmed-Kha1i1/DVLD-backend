using DVLD.Application.Common.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
namespace ApiLayer.Validators
{
    public class ValidationResultFactory : ResponseHandler, IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            var response = ValidationError(validationProblemDetails?.Errors, "One or more Valodation error occurred.");
            var result = new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };

            return result;
        }
    }
}

