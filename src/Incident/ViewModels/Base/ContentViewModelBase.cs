using System;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace Incident.ViewModels.Base;

internal abstract class ContentViewModelBase : ViewModelBase
{
    protected ContentViewModelBase(ILogger logger) : base(logger)
    {
        BringToViewRequestCommand = new RelayCommand(RequestBringToView);
    }

    public virtual bool HasSideContent => true;

    private bool _fileMenuVisible;

    public bool FileMenuAccessible => HasSideContent;

    public bool FileMenuVisible
    {
        get => _fileMenuVisible;
        set => SetProperty(ref _fileMenuVisible, value);
    }

    public virtual string? FileMenuHeader => null;

    protected static bool IsPropertyAffected(PropertyChangedEventArgs e, string propertyName)
        => string.IsNullOrEmpty(e.PropertyName) || string.Equals(e.PropertyName, propertyName, StringComparison.Ordinal);

    public ICommand BringToViewRequestCommand { get; }

    public event EventHandler<EventArgs>? BringToViewRequested;

    private void RequestBringToView() => BringToViewRequested?.Invoke(this, EventArgs.Empty);
}