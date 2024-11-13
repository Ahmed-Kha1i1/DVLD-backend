using DVLD.Application;
using DVLD.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DVLD.API
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
            // AddAsync API documentation and exploration
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            Services.AddApplicationServices(_configuration)
                .AddPersistenceServices(_configuration)
                .AddInfrastructureServices(_configuration);

            Services.AddProblemDetails();
            Services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesResponseTypeAttribute(401));
                options.Filters.Add(new ProducesResponseTypeAttribute(403));
                options.Filters.Add(new ProducesResponseTypeAttribute(500));
            });
            Services.AddSingleton<FileExtensionContentTypeProvider>();
            // Configure CORS
            Services.AddCors(options =>
            {
                options.AddPolicy(AllowSpicificOrigin, policy =>
                {
                    policy.WithOrigins("https://licensemanagment.netlify.app").AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("Location");
                });
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

            //app.UseAuthorization();
            app.MapControllers();
        }
    }
}
