using Platform.Contracts;
using Picsum.Services;

namespace App.Web;
public static class ServiceExtensions
{
    public static void AddPlatformServices(this IServiceCollection services)
    {
        services.AddTransient<IStreamService<IPhoto>, PicsumStreamService>();
    }
}