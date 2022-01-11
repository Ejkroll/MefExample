using MefExample.Core;

namespace MefExample.Plugin2
{
    public class Startup : IPluginStartup
    {
        public void ConfigureServices(IPluginRegistrar pluginRegistrar)
        {
            pluginRegistrar.AddScoped<IPluginService, PluginService2>();
        }
    }
}
