namespace DVLD.Application.Features.LocalApplication.Commands.AddLocalApplicationCommand
{
    public class AddLocalApplicationCommandValidator : AbstractValidator<AddLocalApplicationCommand>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILicenseRepository _licenseRepository;
        private readonly ILicenseClassRepository _licenseClassRepository;
        public AddLocalApplicationCommandValidator(IApplicationRepository applicationRepository,
      ILicenseRepository licenseRepository, ILicenseClassRepository licenseClassRepository)
        {
            _applicationRepository = applicationRepository;
            _licenseRepository = licenseRepository;
            _licenseClassRepository = licenseClassRepository;

            RuleFor(obj => obj.LicenseClassId).ValidId();
            RuleFor(obj => obj.PersonId).ValidId();
            RuleFor(obj => obj.UserId).ValidId();

            RuleFor(x => x)
              .MustAsync(async (command, cancellation) =>
                  await _licenseClassRepository.IsPersonAgeValidForLicenseAsync(command.PersonId, command.LicenseClassId))
              .WithMessage("Person does not meet the minimum age requirement for this license class.")
              .MustAsync(async (command, cancellation) =>
                  await IsActiveApplicationAbsent(command, cancellation))
              .WithMessage("This person already has an active application for the selected class.")
              .MustAsync(async (command, cancellation) =>
                  !await _licenseRepository.IsLicenseExist(command.PersonId, command.LicenseClassId))
              .WithMessage("Person already has a license with the same applied driving class. Choose a different driving class.");


        }


        private async Task<bool> IsActiveApplicationAbsent(AddLocalApplicationCommand command, CancellationToken cancellationToken)
        {
            int? activeApplicationId = await _applicationRepository.GetActiveApplicationId(command.PersonId, command.LicenseClassId);
            return activeApplicationId == null;
        }

    }
}
