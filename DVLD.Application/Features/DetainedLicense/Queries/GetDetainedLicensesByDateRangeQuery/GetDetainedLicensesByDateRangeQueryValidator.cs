namespace DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicensesByDateRangeQuery
{
    public class GetDetainedLicensesByDateRangeQueryValidator : AbstractValidator<GetDetainedLicensesByDateRangeQuery>
    {
        public GetDetainedLicensesByDateRangeQueryValidator()
        {

            RuleFor(x => x.StartDate)
              .LessThanOrEqualTo(x => x.EndDate).WithMessage("StartDate must be less than or equal to EndDate.")
              .When(x => x.StartDate.HasValue && x.EndDate.HasValue);

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("EndDate must be greater than or equal to StartDate.")
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
            RuleFor(x => x.StartDate)
                .NotNull().When(x => x.EndDate.HasValue).WithMessage("StartDate is required when EndDate is provided.")
                ;
            RuleFor(x => x.EndDate)
                .NotNull().When(x => x.StartDate.HasValue).WithMessage("EndDate is required when StartDate is provided.");
        }
    }
}
