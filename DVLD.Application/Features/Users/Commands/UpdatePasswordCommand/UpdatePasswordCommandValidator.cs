using ApiLayer.Validators;
using DVLD.Application.Features.Users.Common.Validators;
using FluentValidation;

namespace DVLD.Application.Features.Users.Commands.UpdatePasswordCommand
{
    public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
    {
        public UpdatePasswordCommandValidator()
        {
            RuleFor(user => user.NewPassword).Requeired().ValidPassword();
            RuleFor(user => user.UserId).Requeired().ValidId();
        }
    }
}
