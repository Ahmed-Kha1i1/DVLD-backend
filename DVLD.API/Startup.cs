using DVLD.Application;
using DVLD.Persistence;
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
            Services.AddApplicationServices(_configuration)
                .AddPersistenceServices(_configuration)
                .AddInfrastructureServices(_configuration);

            Services.AddProblemDetails();
            Services.AddControllers();
            Services.AddSingleton<FileExtensionContentTypeProvider>();
            // Configure CORS
            Services.AddCors(options =>
            {
                options.AddPolicy(AllowSpicificOrigin, policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("Location");
                });
            });


            // AddAsync API documentation and exploration
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();
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
