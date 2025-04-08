using Microsoft.Extensions.DependencyInjection;

namespace Incident.Common.Composition;

public abstract class ServiceModuleBase
{
    public abstract void Load(IServiceCollection services);
}