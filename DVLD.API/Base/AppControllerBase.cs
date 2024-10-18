using DVLD.Application.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Base
{
    public class AppControllerBase : ControllerBase
    {
        private IMediator _mediatorInstance;
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        public ObjectResult CreateResult<T>(Response<T> response)
        {
            var result = new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };

            return result;
        }

    }
}
