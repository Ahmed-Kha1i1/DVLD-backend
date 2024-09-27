using ApiLayer.Services;
using FluentValidation;

public static class PersonValidationRules
{
    public static IRuleBuilderOptions<T, string> ValidateUniqueEmail<T>(this IRuleBuilder<T, string> ruleBuilder, PersonService personService, IHttpContextAccessor? accessor = null)
    {
        return ruleBuilder.MustAsync(async (email, cancellation) =>
        {
            int? Id = accessor is not null ? GetPersonIdFromRoute(accessor) : null;
            return await personService.BeUniqueEmail(email, Id);
        }).WithMessage("Email must be unique");

    }

    public static IRuleBuilderOptions<T, string> ValidateUniquePhone<T>(this IRuleBuilder<T, string> ruleBuilder, PersonService personService, IHttpContextAccessor? accessor = null)
    {
        return ruleBuilder.MustAsync(async (Phone, cancellation) =>
        {
            int? Id = accessor is not null ? GetPersonIdFromRoute(accessor) : null;
            return await personService.BeUniquePhone(Phone, Id);
        }).WithMessage("Phone must be unique");

    }

    public static IRuleBuilderOptions<T, string> ValidateUniqueNationalNo<T>(this IRuleBuilder<T, string> ruleBuilder, PersonService personService, IHttpContextAccessor? accessor = null)
    {
        return ruleBuilder.MustAsync(async (nationalNumber, cancellation) =>
        {
            int? Id = accessor is not null ? GetPersonIdFromRoute(accessor) : null;
            return await personService.BeUniqueNationalNo(nationalNumber, Id);
        }).WithMessage("National No must be unique");

    }

    public static int? GetPersonIdFromRoute(IHttpContextAccessor accessor)
    {
        var PersonIdString = accessor.HttpContext?.Request.RouteValues["personId"]?.ToString();

        if (int.TryParse(PersonIdString, out int PersonId))
        {
            return PersonId;
        }

        return null;
    }
}