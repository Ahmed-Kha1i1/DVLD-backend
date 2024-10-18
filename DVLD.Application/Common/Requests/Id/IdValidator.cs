using ApiLayer.Validators;
using FluentValidation;

namespace DVLD.Application.Common.Requests.Id
{
    public class IdValidator : AbstractValidator<IdRequest>
    {
        public IdValidator()
        {
            RuleFor(obj => obj.Id).ValidId();
        }
    }
}
