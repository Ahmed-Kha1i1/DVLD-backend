using ApiLayer.Validators;
using FluentValidation;

namespace DVLD.Application.Features.ApplicationType.Commands.UpdateApplicationTypeCommand
{
    public class UpdateApplicationTypeCommandValidator : AbstractValidator<UpdateApplicationTypeCommand>
    {
        public UpdateApplicationTypeCommandValidator()
        {
            RuleFor(appType => appType.ApplicationTypeId).Requeired().ValidId();
            RuleFor(appType => appType.Title).Requeired();
            RuleFor(appType => appType.Fees).Requeired();
        }
    }
}
