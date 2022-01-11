using MefExample.Core;
using Microsoft.Extensions.DependencyInjection;

namespace MefExample.Api.Middleware.Plugins
{
    public class PluginRegistrar : IPluginRegistrar
    {
        private readonly IServiceCollection _serviceCollection;
        public PluginRegistrar(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }


        public void AddScoped<TService>() where TService : class
        {
            _serviceCollection.AddScoped<TService>();
        }
        public void AddScoped<TService, TImplemetation>() where TService : class where TImplemetation : class, TService
        {
            _serviceCollection.AddScoped<TService, TImplemetation>();
        }
    }
}
