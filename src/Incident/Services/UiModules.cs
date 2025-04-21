using Incident.Common.Composition;
using Incident.Composition;
using Incident.ViewModels;
using Incident.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Incident.Services;

internal class UiModules : ServiceModuleBase
{
    public override void Load(IServiceCollection services)
    {
        services.AddSingleton<IViewLocator, ViewLocator>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IMainViewModel, WelcomeViewModel>();
        services.AddSingleton<MainMenuViewModel>();
    }
}