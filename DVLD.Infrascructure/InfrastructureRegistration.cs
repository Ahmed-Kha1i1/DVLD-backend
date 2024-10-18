using DVLD.Application.Infrastracture;
using DVLD.Application.Services.ImageService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace DVLD.Application
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //register services 
            services.AddScoped<IImageService, ImageService>();
            return services;
        }
    }
}
