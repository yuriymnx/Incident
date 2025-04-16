using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Incident.Common;
using Incident.Common.Services.Interfaces;
using Incident.Composition;
using Incident.Core.Configuration;
using Incident.Core.Configuration.Application;
using Incident.Core.Configuration.Interfaces;
using Incident.Core.Configuration.User;
using Incident.Utils;
using Incident.ViewModels.Base;
using Incident.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Incident;

public class App : Application
{
    private IHost? _host;

    private bool _startupFailed;

    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
            if (_host == null)
            {
                Environment.Exit(-1);
            }
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
        LogManager.Configuration.Variables["userLevel"] = _host.Services.GetRequiredService<IAppConfiguration>().LogLevel;
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            BindingPlugins.DataValidators.RemoveAt(0);
            _host.Services.GetRequiredService<IEnvironmentInfo>().Dump();
            desktop.MainWindow = _host.Services.GetRequiredService<MainWindow>();
            var mainViewModel = _host.Services.GetRequiredService<IMainViewModel>();
            var logger = LogManager.GetCurrentClassLogger();
            desktop.Exit += async (s, e) =>
            {
                logger.Trace("Exeting application");
                if (mainViewModel is IDisposable d)
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
        services.Configure<UserSettings>(context.Configuration);
        if (context.Properties.TryGetValue(typeof(AppConfigurationSource<UserSettings>), out var value) &&
            value is AppConfigurationProvider<UserSettings> userConfigSource)
        {
            services.AddSingleton(userConfigSource);
        }
        services.AddSingleton<MainWindow>();
       
        services.LoadAllModules();
    }

    private void SetupConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
    {
        Directory.CreateDirectory(AppConfigurationProvider.ConfigDirectory);
        builder.Add(new AppConfigurationSource<AppSettings>(Constants.AppConfigFileName));
        var ucs = new AppConfigurationSource<UserSettings>(Constants.UserConfigFileName);
        builder.Add(ucs);
        context.Properties.Add(typeof(AppConfigurationProvider<UserSettings>), ucs);
    }
}