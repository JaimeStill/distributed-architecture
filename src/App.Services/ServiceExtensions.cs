using Microsoft.Extensions.DependencyInjection;
using Platform.Contracts;

namespace App.Services;
public static class ServiceExtensions
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IService<>), typeof(ServiceBase<>));
        services.AddScoped<PhotoService>();
    }
}