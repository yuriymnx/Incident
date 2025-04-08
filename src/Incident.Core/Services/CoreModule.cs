using Incident.Common.Composition;
using Microsoft.Extensions.DependencyInjection;

namespace Incident.Core.Services;

public sealed class CoreModule : ServiceModuleBase
{
    public override void Load(IServiceCollection services)
    {
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