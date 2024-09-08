using DVLDApi.Profiles;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DVLDApi
{
    public static class SetupExtensions
    {
        
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(Assembly.Load("BusinessLayer"));
            builder.Services.AddProblemDetails();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            return builder.Build();
        }

        public static WebApplication ConfigurePipline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
