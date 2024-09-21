using DVLDApi.Profiles;
using Microsoft.AspNetCore.StaticFiles;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DVLDApi
{
    public static class SetupExtensions
    {
        static string AllowSpicificOrigin = "_AllowSpicificOrigin";
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(Assembly.Load("BusinessLayer"));
            builder.Services.AddProblemDetails();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AllowSpicificOrigin, policy =>
                {
                    policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
                });
            });
            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
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

            app.UseCors(AllowSpicificOrigin);

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
