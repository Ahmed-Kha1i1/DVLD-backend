
namespace DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationPerTestTypeQuery
{
    public class GetLocalApplicationPerTestTypeQueryValidator : AbstractValidator<GetLocalApplicationPerTestTypeQuery>
    {
        public GetLocalApplicationPerTestTypeQueryValidator()
        {
            RuleFor(x => x.TestTypeId).IsInEnum();
            RuleFor(x => x.LocalApplicationId).ValidId();
        }
    }
}
