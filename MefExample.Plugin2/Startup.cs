using MefExample.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MefExample.Plugin2;

public class Startup : IPluginStartup
{
    public IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPluginService, PluginService2>();

        return services;
    }
}
