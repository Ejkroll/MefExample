
namespace MefExample.Core
{
    public interface IPluginRegistrar
    {
        void AddScoped<TService>() where TService : class;
        void AddScoped<TService, TImplemetation>() where TService : class where TImplemetation : class, TService;
    }
}
