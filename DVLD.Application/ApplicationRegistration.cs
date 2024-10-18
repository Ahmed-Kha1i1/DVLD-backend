using ApiLayer.Validators;
using DVLD.Application.Exceptions.Handlers;
using DVLD.Application.Models;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;


namespace DVLD.Application
{
    public static class ApplicationRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.Configure<ImagesOptions>(configuration.GetSection("ImageUpload"));

            // Configure Fluent Validation
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies(), ServiceLifetime.Scoped);
            services.AddFluentValidationAutoValidation(config =>
            {
                config.DisableBuiltInModelValidation = true;
                config.EnableBodyBindingSourceAutomaticValidation = true;
                config.EnableFormBindingSourceAutomaticValidation = true;
                config.EnableQueryBindingSourceAutomaticValidation = true;
                config.EnablePathBindingSourceAutomaticValidation = true;
                config.OverrideDefaultResultFactoryWith<ValidationResultFactory>();
            });
            return services;
        }
    }
}
