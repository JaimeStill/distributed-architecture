using Microsoft.Extensions.DependencyInjection;
using Picsum.Services;
using Platform.Contracts;

namespace Platform.Broker;
public static class ServiceBroker
{
    public static IServiceCollection RegisterPicsumService(this IServiceCollection services) =>
        services.AddTransient<IStreamService<IPhoto>, PicsumStreamService>();
}