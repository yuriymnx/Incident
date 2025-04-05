using Incident.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Incident;

internal abstract class MainViewModelBase : ViewModelBase, IRootContent, IMainViewModel
{
    private readonly ObservableCollection<IRootContent> _rootMessages = new();

    private readonly ObservableCollection<IRootContent> _topMessages = new();

    protected MainViewModelBase(ILogger<MainViewModelBase> logger)
        : base(logger)
    {
    }

    public bool IsModal => false;

    public abstract Task Startup(string[]? args);

    public IReadOnlyList<IRootContent> RootMessages => _rootMessages;
    public IReadOnlyList<IRootContent> TopMessages => _topMessages;

    public bool HasTopMessages => _topMessages.Count > 0;
    public IRootContent? TopDialogMessage => _topMessages.Count == 0 ? null : _topMessages[^1];
    
    private bool _allDialogsVisible;
    public bool AllDialogsVisible
    {
        get => _allDialogsVisible;
        set => SetProperty(ref _allDialogsVisible, value);
    }

    public void ShowDialogMessage(IRootContent message)
    {
        _topMessages.Add(message);
        Logger.LogDebug("Added dialog message: {0}, top messages count: {1}", message, _topMessages.Count);
        OnPropertyChanged(nameof(HasTopMessages));
    }

    public void ReplaceDialogMessage(IRootContent message)
    {
        if (_topMessages.Count == 0)
        {
            ShowDialogMessage(message);
            return;
        }
        var oldMessage = _topMessages[^1];
        _topMessages[^1] = message;
        if (oldMessage is IDisposable d)
        {
            d.Dispose();
        }
    }

    public void RemoveDialogMessage()
    {
        var last = _topMessages.LastOrDefault();
        if (last == null)
        {
            return;
        }
        _topMessages.RemoveAt(_topMessages.Count - 1);
        if (last is IDisposable d)
        {
            d.Dispose();
        }
        OnPropertyChanged(nameof(HasTopMessages));
        Logger.LogDebug("Top messages count: {0}", _topMessages.Count);
    }

    public void ShowRootMessage(IRootContent message)
    {
        _rootMessages.Add(message);
        Logger.LogDebug("Added dialog message: {0}, top messages count: {1}", message, _rootMessages.Count);
    }

    public void ReplaceRootMessage(IRootContent message)
    {
        if (_rootMessages.Count == 0)
        {
            ShowRootMessage(message);
            return;
        }

        var oldMessage = _rootMessages[^1];
        if (_rootMessages.Count == 1 && oldMessage is MainViewModelBase)
        {
            return;
        }
        _topMessages[^1] = message;
        if (oldMessage is IDisposable d)
        {
            d.Dispose();
        }
    }

    public void RemoveRootMessage()
    {
        var last = _rootMessages.LastOrDefault();
        if (last == null || _rootMessages.Count == 0 || (_rootMessages.Count == 1 && last is MainViewModelBase))
        {
            return;
        }
        _rootMessages.RemoveAt(_topMessages.Count - 1);
        if (last is IDisposable d)
        {
            d.Dispose();
        }
        OnPropertyChanged(nameof(HasTopMessages));
        Logger.LogDebug("Root messages count: {0}", _rootMessages.Count);
    }
}