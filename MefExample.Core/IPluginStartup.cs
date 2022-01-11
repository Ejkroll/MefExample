using System.ComponentModel.Composition;

namespace MefExample.Core
{
    /*
     * this is what actually allows us to import the plugins. 
     * you could do this directly at the service level instead
     * of at the DI startup registration
     */
    [InheritedExport(typeof(IPluginStartup))]
    public interface IPluginStartup
    {
        void ConfigureServices(IPluginRegistrar registrar);
    }
}
