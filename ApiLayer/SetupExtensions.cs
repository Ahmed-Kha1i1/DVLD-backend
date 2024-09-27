using ApiLayer.Configurations;
using ApiLayer.ExceptionsHandlers;
using ApiLayer.Services;
using ApiLayer.Validators;
using FluentValidation;
using Microsoft.AspNetCore.StaticFiles;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;

namespace DVLDApi
{
    public class Startup
    {
        private string AllowSpicificOrigin = "_AllowSpicificOrigin";
        private readonly IConfigurationRoot _configuration;
        public Startup(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection Services)
        {
            // Add framework services
            Services.AddProblemDetails();
            Services.AddControllers();
            Services.AddHttpContextAccessor();

            // Configure CORS
            Services.AddCors(options =>
            {
                options.AddPolicy(AllowSpicificOrigin, policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("Location");
                });
            });

            // Configure AutoMapper
            Services.AddAutoMapper(Assembly.Load("BusinessLayer"), Assembly.GetExecutingAssembly());

            // Configure Image Upload Options
            Services.Configure<ImagesOptions>(_configuration.GetSection("ImageUpload"));

            // Register services
            Services.AddSingleton<FileExtensionContentTypeProvider>();
            Services.AddSingleton<ImageService>();
            Services.AddSingleton<PersonService>();

            // Register Exceptions Handlers
            Services.AddExceptionHandler<GlobalExceptionHandler>();

            // Add API documentation and exploration
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();

            // Configure Fluent Validation
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Singleton);
            Services.AddFluentValidationAutoValidation(config =>
            {
                config.DisableBuiltInModelValidation = true;
                config.EnableBodyBindingSourceAutomaticValidation = true;
                config.EnableFormBindingSourceAutomaticValidation = true;
                config.EnableQueryBindingSourceAutomaticValidation = true;
                config.OverrideDefaultResultFactoryWith<ValidationResultFactory>();
            });
        }

        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseCors(AllowSpicificOrigin);

            app.UseAuthorization();
            app.MapControllers();
        }
    }
}

