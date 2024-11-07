using ApiLayer.Validators;
using FluentValidation;

namespace DVLD.Application.Features.TestType.Commands.UpdateTestTypeCommand
{
    public class UpdateTestTypeCommandValidator : AbstractValidator<UpdateTestTypeCommand>
    {
        public UpdateTestTypeCommandValidator()
        {
            RuleFor(testTye => testTye.TestTypeId).Requeired().ValidId();
            RuleFor(testTye => testTye.Title).Requeired();
            RuleFor(testTye => testTye.Description).Requeired();
            RuleFor(testTye => testTye.Fees).Requeired();
        }
    }
}
