using AutoMapper.Data;
using DVLD.Application.Contracts.Persistence;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DVLD.Persistence
{
    public static class PersistenceRegistraction
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<DataSendhandler>();

            services.AddAutoMapper(ctg =>
            {
                ctg.AddDataReaderMapping();
            });
            //register Repositories
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>();
            services.AddScoped<IDetainedLicenseRepository, DetainedLicenseRepository>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IInternationalLicenseRepository, InternationalLicenseRepository>();
            services.AddScoped<ILicenseClassRepository, LicenseClassRepository>();
            services.AddScoped<ILicenseRepository, LicenseRepository>();
            services.AddScoped<ILocalApplicationRepository, LocalApplicationRepository>();
            services.AddScoped<ITestAppointmentRepository, TestAppointmentRepository>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<ITestTypeRepository, TestTypeRepository>();
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();


            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
            return services;
        }

    }
}
