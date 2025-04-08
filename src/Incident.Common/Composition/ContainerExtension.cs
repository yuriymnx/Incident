using Microsoft.Extensions.DependencyInjection;

namespace Incident.Common.Composition;

public static class ContainerExtension
{
    public static void Register(this IServiceCollection services, params ServiceModuleBase[] modules)
    {
        foreach (var module in modules)
        {
            module.Load(services);
        }
    }
}