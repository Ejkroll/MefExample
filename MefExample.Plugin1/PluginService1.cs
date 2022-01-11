using MefExample.Core;

namespace MefExample.Plugin1
{
    public class PluginService1 : IPluginService
    {
        public virtual string DoSomething()
        {
            return "I did something cool in PluginService1";
        }
    }
}
