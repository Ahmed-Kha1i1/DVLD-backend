namespace DVLD.Application.Features.TestAppointment.Common.Requests.ListPerTestTypeRequest
{
    public class LocalAppTestTypeRequestValidator : AbstractValidator<LocalAppTestTypeRequest>
    {
        public LocalAppTestTypeRequestValidator()
        {
            RuleFor(x => x.TestTypeId).IsInEnum();
            RuleFor(x => x.LocalApplicationId).ValidId();
        }
    }
}
