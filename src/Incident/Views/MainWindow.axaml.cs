using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Incident.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Incident.Views;

public partial class MainWindow : Window
{
    private readonly ILogger<MainWindow> _logger;

    public MainWindow(ILogger<MainWindow> logger)
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif
        _logger = logger;
        _logger.LogDebug("CurrentDirectory: {0}", Directory.GetCurrentDirectory());
        _logger.LogDebug("AppDomain: {0}", AppDomain.CurrentDomain.BaseDirectory);
        Loaded += MainWindowLoaded;
    }

    private void MainWindowLoaded(object? sender, RoutedEventArgs e)
    {
        Loaded += MainWindowLoaded;
        Dispatcher.UIThread.InvokeAsync(Initialize, DispatcherPriority.ContextIdle);
    }

    private async Task Initialize()
    {
        await UiDispatcher.Yield();
        if(Application.Current is not App app || !app.EnsureStarted()) 
            return;
        if (DataContext is not IMainViewModel mvm)
        {
            _logger.LogError("Main window data context is not an instance of IMainViewModel");
            DataContext = null;
            Content = null;
            return;
        }
        
        await mvm.Startup((Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Args);
    }
}