using Incident.Common.Composition;
using Incident.Common.Services;
using Incident.Common.Services.Interfaces;
using Incident.Core.Configuration.Application;
using Incident.Core.Configuration.Interfaces;
using Incident.Core.Configuration.User;
using Microsoft.Extensions.DependencyInjection;

namespace Incident.Core.Services;

public sealed class CoreModule : ServiceModuleBase
{
    public override void Load(IServiceCollection services)
    {
        services.AddSingleton<IEnvironmentInfo, EnvironmentInfo>();
        services.AddSingleton<IAppConfiguration, AppConfiguration>();
        services.AddSingleton<IUserConfiguration, UserConfiguration>();
        services.AddSingleton<IUserService, UserService>();
    }
}


// Sample TODO: Delete
public interface IUserService
{
    string GetUserName();
}

internal class UserService : IUserService
{
    public string GetUserName() => Environment.UserName;
}