

namespace DVLD.Application.Features.LocalApplication.Commands.IssueLocalApplicationCommand
{
    public class IssueLocalApplicationCommandValidator : AbstractValidator<IssueLocalApplicationCommand>
    {
        private readonly ITestRepository _testRepository;
        private readonly ILocalApplicationRepository _localApplicationRepository;
        public IssueLocalApplicationCommandValidator(ITestRepository testRepository, ILocalApplicationRepository localApplicationRepository)
        {
            _testRepository = testRepository;
            _localApplicationRepository = localApplicationRepository;

            RuleFor(obj => obj.LocalApplicationId).ValidId();
            RuleFor(obj => obj.userId).ValidId();

            RuleFor(command => command.LocalApplicationId)
                .MustAsync(async (localApplicationId, cancellationToken) => await PassedAllTests(localApplicationId))
                .WithMessage("Person should pass all tests first.");

            RuleFor(command => command.LocalApplicationId)
                .MustAsync(async (localApplicationId, cancellationToken) => !await HasActiveLicense(localApplicationId))
                .WithMessage("Person already has a license with the selected application.");
        }

        private async Task<bool> PassedAllTests(int localApplicationId)
        {
            return await _testRepository.PassedAllTests(localApplicationId);
        }

        private async Task<bool> HasActiveLicense(int localApplicationId)
        {
            var activeLicenseId = await _localApplicationRepository.GetActiveLicenseID(localApplicationId);
            return activeLicenseId != null; // true if active license exists
        }
    }
}
