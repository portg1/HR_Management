using System.Reflection;
using HR_Management.Application.Profiles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HR_Management.Application;

public static class ApplicationServicesRegistration
{
    public static void ConfigureApplicationServices(this IServiceCollection services)
    {
       // services.AddAutoMapper(typeof(MappingProfile));
       services.AddAutoMapper(Assembly.GetExecutingAssembly());
       services.AddMediatR(configuration =>
           configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


	}

}
