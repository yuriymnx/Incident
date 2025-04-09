using System;
using System.Linq;
using System.Reflection;
using Incident.Common.Composition;
using Microsoft.Extensions.DependencyInjection;

namespace Incident.Utils;

public static class ServiceModuleLoader
{
    public static void LoadAllModules(this IServiceCollection services, Assembly[]? assemblies = null)
    {
        assemblies ??= AppDomain.CurrentDomain.GetAssemblies();

        var moduleTypes = assemblies
            .SelectMany(asm => asm.GetTypes())
            .Where(type =>
                typeof(ServiceModuleBase).IsAssignableFrom(type) &&
                type is { IsClass: true, IsAbstract: false } &&
                type.GetConstructor(Type.EmptyTypes) != null)
            .ToList();

        foreach (var module in moduleTypes.Select(type => (ServiceModuleBase)Activator.CreateInstance(type)!))
        {
            module.Load(services);
        }
    }
}