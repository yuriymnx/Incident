using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Input;

namespace Incident.ViewModels.Base;

internal abstract class ViewModelBase : ObservableObject
{
    internal ViewModelBase(ILogger logger)
    {
        Logger = logger;
        CloseRequestedCommand = new RelayCommand(RequestClose);
    }

    protected ILogger Logger { get; }

    public ICommand CloseRequestedCommand { get; }

    public event EventHandler<EventArgs>? CloseRequested;

    private void RequestClose() => CloseRequested?.Invoke(this, EventArgs.Empty);
}
