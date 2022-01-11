using MefExample.Core;

namespace MefExample.Plugin2
{
    public class PluginService2 : IPluginService
    {
        public virtual string DoSomething()
        {
            return "I did something even cooler in PluginService2";
        }
    }
}
