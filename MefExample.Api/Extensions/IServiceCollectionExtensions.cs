using MefExample.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MefExample.Api;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPlugins(this IServiceCollection services, IConfiguration configuration)
    {
        var importDef = BuildImportDefinition();
        try
        {
            var pluginDirectory = configuration.GetValue<string>("PluginDirectory");
            ArgumentException.ThrowIfNullOrWhiteSpace(pluginDirectory, "Plugin directory not found in configuration");

            using var agCat = new AggregateCatalog();

            GetAllDirectories(pluginDirectory).ForEach(d =>
            {
                agCat.Catalogs.Add(new DirectoryCatalog(d));
            });

            using var container = new CompositionContainer(agCat);
            
            IEnumerable<Export> exports = container.GetExports(importDef);
            IEnumerable<IPluginStartup?> modules = exports.Select(e => e.Value as IPluginStartup).Where(m => m != null);

            // trigger startup registrations
            foreach(var module in modules)
                services = module?.ConfigureServices(services, configuration) ?? services;
        }
        catch (ReflectionTypeLoadException typeLoadExecption)
        {
            var builder = new StringBuilder();
            foreach (var ex in typeLoadExecption.LoaderExceptions)
                builder.AppendFormat("{0}\n", ex?.Message);
            throw new TypeLoadException(builder.ToString(), typeLoadExecption);
        }
        return services;
    }
    private static ImportDefinition BuildImportDefinition() => new (def => true, typeof(IPluginStartup).FullName, ImportCardinality.ZeroOrMore, false, false);
    private static List<string> GetAllDirectories(string directoryPath)
    {
        var asm = Assembly.GetExecutingAssembly();
        var root = new FileInfo(asm.Location).DirectoryName;
        ArgumentException.ThrowIfNullOrWhiteSpace(root, "Could not find root directory");

        var path = Path.Combine(root, directoryPath);
        var ret = new List<string>() { path };
        foreach (var dir in Directory.GetDirectories(path))
            ret.Add(dir);

        return ret;
    }
}
