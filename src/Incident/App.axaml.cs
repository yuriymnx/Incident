using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Incident.Common.Services.Interfaces;
using Incident.Composition;
using Incident.Core.Configuration;
using Incident.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Incident;

public partial class App : Application
{
    private IHost? _host;

    private bool _startupFailed;

    private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        try
        {
            _startupFailed = false;
            await InitServices();
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex);
            _startupFailed = true;
        }
    }

    public bool EnsureStarted()
    {
        if (!_startupFailed)
            return true;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            desktop.Shutdown();
        }
        else
        {
            Environment.Exit(-1);
        }

        return false;
    }

    private async Task InitServices()
    {
        _host = Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
            .ConfigureAppConfiguration(SetupConfiguration)
            .ConfigureServices(SetupServices)
            .Build();
        NLog.LogManager.Configuration.Variables["userLevel"] = _host.Services.GetRequiredService<IAppConfiguration>().LogLevel;
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            BindingPlugins.DataValidators.RemoveAt(0);
            _host.Services.GetRequiredService<IEnvironmentInfo>().Dump();
            desktop.MainWindow = _host.Services.GetRequiredService<MainWindow>();
            var mvm = _host.Services.GetRequiredService<IMainViewModel>(); 
            var logger = NLog.LogManager.GetCurrentClassLogger();
            desktop.Exit += async (s, e) =>
            {
                logger.Trace("Exeting application");
                if (mvm is IDisposable d)
                {
                    d.Dispose();
                    logger.Trace("IMainViewModel dispose");
                }
                await _host.StopAsync(TimeSpan.FromSeconds(5));
                logger.Trace("host stopped");
                _host.Dispose();
                logger.Trace("host disposed");
                _host = null;
            };
        }

        DataTemplates.Add(_host.Services.GetRequiredService<IViewLocator>());
        base.OnFrameworkInitializationCompleted();
        await _host.StartAsync();
    }

    private void SetupServices(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<AppSettings>(context.Configuration);
        services.Configure<UserSettings>(context.Configuration)
    }

    private void SetupConfiguration(HostBuilderContext context, IServiceCollection services)
    {
        throw new NotImplementedException();
    }
}