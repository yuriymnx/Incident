using Incident.Common.Composition;
using Microsoft.Extensions.DependencyInjection;
using System;
using Incident.Composition;
using Incident.ViewModels;
using Incident.ViewModels.Base;

namespace Incident.Services;

internal class UiModules : ServiceModuleBase
{
    public override void Load(IServiceCollection services)
    {
        services.AddSingleton<IViewLocator, ViewLocator>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IMainViewModel, WelcomeViewModel>();
    }
}