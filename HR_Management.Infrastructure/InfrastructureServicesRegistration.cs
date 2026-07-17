using HR_Management.Application.Contracts.Infrastructure;
using HR_Management.Application.Models;
using HR_Management.Infrastructure.Cache;
using HR_Management.Infrastructure.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HR_Management.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EmailSetting>(configuration.GetSection("EmailSetting"));
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = configuration["Redis:InstanceName"];
        });

        services.AddScoped<ICacheService, RedisCacheService>();
        return services;
        
    }

}