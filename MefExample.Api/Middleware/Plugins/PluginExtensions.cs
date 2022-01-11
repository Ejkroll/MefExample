using MefExample.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MefExample.Api.Middleware.Plugins
{
    public static class PluginExtensions
    {
        // this plugin registration allows for a plugin to self register in the DI container.
        public static IServiceCollection AddPlugins(this IServiceCollection services, string pluginDirectory)
        {
            var importDef = BuildImportDefinition();
            try
            {
                using (var agCat = new AggregateCatalog())
                {
                    GetAllDirectories(pluginDirectory).ForEach(d =>
                    {
                        agCat.Catalogs.Add(new DirectoryCatalog(d));
                    });

                    using (var container = new CompositionContainer(agCat))
                    {
                        IEnumerable<Export> exports = container.GetExports(importDef);
                        IEnumerable<IPluginStartup> modules = exports.Select(e => e.Value as IPluginStartup).Where(m => m != null);

                        var component = new PluginRegistrar(services);
                        foreach (var module in modules)
                            module.ConfigureServices(component);
                    }
                }
            }
            catch (ReflectionTypeLoadException typeLoadExecption)
            {
                var builder = new StringBuilder();
                foreach (var ex in typeLoadExecption.LoaderExceptions)
                    builder.AppendFormat("{0}\n", ex.Message);
                throw new TypeLoadException(builder.ToString(), typeLoadExecption);
            }
            return services;
        }
        private static ImportDefinition BuildImportDefinition()
        {
            return new ImportDefinition(def => true, typeof(IPluginStartup).FullName, ImportCardinality.ZeroOrMore, false, false);
        }
        private static List<string> GetAllDirectories(string directoryPath)
        {
            var asm = Assembly.GetExecutingAssembly();
            var root = new FileInfo(asm.Location).DirectoryName;
            var path = Path.Combine(root, directoryPath);
            var ret = new List<string>() { path };
            foreach (var dir in Directory.GetDirectories(path))
                ret.Add(dir);
            return ret;
        }
    }
}
