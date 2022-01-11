using MefExample.Core;

namespace MefExample.Plugin1
{
    public class Startup : IPluginStartup
    {
        public void ConfigureServices(IPluginRegistrar pluginRegistrar)
        {
            pluginRegistrar.AddScoped<IPluginService, PluginService1>();
        }
    }
}
