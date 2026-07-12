using System.Reflection;
using HR_Management.Application.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace HR_Management.Application;

public static class ApplicationServicesRegistration
{
    public static void ApplicationServices(this IServiceCollection services)
    {
       // services.AddAutoMapper(typeof(MappingProfile));
       services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
    
    
}