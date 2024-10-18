using ApiLayer.Validators;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.People.Commands.ModificationPerson;
using DVLD.Application.Features.People.Common.Validators;
using DVLD.Application.Models;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace DVLD.Application.Features.People.Commands.UpdatePersonCommand
{
    public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
    {
        public UpdatePersonCommandValidator(IOptionsSnapshot<ImagesOptions> imagesOptions, IPersonRepository personRepository)
        {
            Include(new ModificationPersonCommandValidator(imagesOptions));
            RuleFor(person => person.PersonId).Requeired().ValidId();
            RuleFor(person => person.NationalNo)
                .Requeired().ValidNationalNo()
                .MustAsync(async (Model, nationalNumber, cancellation) => await personRepository.IsNationalNoUnique(nationalNumber, Model.PersonId)).WithMessage("NationalNo must be unique");
            RuleFor(person => person.Phone)
                .Requeired().ValidPhone()
                .MustAsync(async (Model, phone, cancellation) => await personRepository.IsPhoneUnique(phone, Model.PersonId)).WithMessage("Phone must be unique");
            RuleFor(person => person.Email)
                .Requeired().ValidEmail()
                .MustAsync(async (Model, nationalNumber, cancellation) => await personRepository.IsEmailUnique(nationalNumber, Model.PersonId)).WithMessage("Email must be unique");
        }
    }
}
