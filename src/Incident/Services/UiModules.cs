using Incident.Common.Composition;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Incident.Services;

internal class UiModules : ServiceModuleBase
{
    public override void Load(IServiceCollection services)
    {
        services.AddSingleton<IDialogService, DialogService>();
    }
}