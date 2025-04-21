using CommunityToolkit.Mvvm.Input;
using Incident.Core.Configuration.Interfaces;
using Incident.ViewModels.Root;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Incident.ViewModels.Base;

internal abstract class MainViewModelBase : ViewModelBase, IRootContent, IMainViewModel
{
    private readonly ObservableCollection<ContentViewModelBase> _documents = new();
    private readonly ObservableCollection<IRootContent> _rootMessages = new();
    private readonly ObservableCollection<IRootContent> _topMessages = new();

    protected MainViewModelBase(
        ILogger<MainViewModelBase> logger,
        MainMenuViewModel mainMenuViewModel,
        IAppConfiguration configuration
    ) : base(logger)
    {
        HomeCommand = new RelayCommand(() => IsContentVisible = false, () => IsContentVisible && MultiFileMode);
        MainMenuData = mainMenuViewModel;
    }

    public abstract Task Startup(string[]? args);

    public IReadOnlyList<ContentViewModelBase> Documents => _documents;

    private ContentViewModelBase? _selectedDocument;
    public ContentViewModelBase? SelectedDocument
    {
        get => _selectedDocument;
        set => SetProperty(ref _selectedDocument, value);
    }

    private bool _isContentVisible;
    public bool IsContentVisible
    {
        get => _isContentVisible;
        set
        {
            if (!SetProperty(ref _isContentVisible, value))
                return;
            UpdateState();
        }
    }

    public bool MultiFileMode { get; }

    public bool HasContent => _documents.Count > 0;

    public bool HasVisibleContent => HasContent && IsContentVisible;

    public bool HasHiddenContent => HasContent && !IsContentVisible;

    public virtual bool RootMessagesVisible => !HasVisibleContent;

    public IRelayCommand HomeCommand { get; }

    public MainMenuViewModel MainMenuData { get; }

    public IReadOnlyList<IRootContent> RootMessages => _rootMessages;

    public IReadOnlyList<IRootContent> TopMessages => _topMessages;

    public bool HasTopMessages => _topMessages.Count > 0;

    private bool _isLoggedIn;

    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        private set
        {
            if (!SetProperty(ref _isLoggedIn, value))
                return;
            UpdateState();
        }
    }

    protected async Task AuthorizeUser()
    {
        IsLoggedIn = false;
        if (HasContent)
            IsContentVisible = false;
        ShowRootMessage(new WaitVm("Loading", true));
        await Task.Delay(100);
        try
        {
            await AuthorizationFinished(null);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Ошибка авторизации пользователя");
            if (await AuthorizationFinished(ex))
                await AuthorizeUser();
        }
        finally
        {
            RemoveRootMessage(); // удаление "Loading"
        }
    }

    private Task<bool> AuthorizationFinished(Exception? exception)
    {
        if (exception == null)
        {
            IsLoggedIn = true;
        }

        // Тут будет обработка ошибок
        return Task.FromResult(false);
    }

    public IRootContent? TopDialogMessage => _topMessages.Count == 0 ? null : _topMessages[^1];

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
            d.Dispose();
    }

    public void ShowRootMessage(IRootContent message)
    {
        _rootMessages.Add(message);
        Logger.LogDebug("Added message: {0}, root messages count: {1}", message, _rootMessages.Count);
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
            return;
        _rootMessages[^1] = message;
        if (oldMessage is IDisposable d)
            d.Dispose();
    }

    public void RemoveDialogMessage()
    {
        var lastMessage = _topMessages.LastOrDefault();
        if (lastMessage == null)
            return;
        _topMessages.RemoveAt(_topMessages.Count - 1);
        if (lastMessage is IDisposable disposable)
            disposable.Dispose();
        OnPropertyChanged(nameof(HasTopMessages));
        Logger.LogDebug("Top messages count: {0}", _topMessages.Count);
    }

    public void RemoveRootMessage()
    {
        var lastMessage = _rootMessages.LastOrDefault();
        if (_rootMessages.Count == 0 || (_rootMessages.Count == 1 && lastMessage is MainViewModelBase))
            return;
        _rootMessages.RemoveAt(_rootMessages.Count - 1);
        if (lastMessage is IDisposable disposable)
            disposable.Dispose();
        Logger.LogDebug("Root messages count: {0}", _rootMessages.Count);
    }

    bool _allDialogsVisible = true;
    public bool AllDialogsVisible
    {
        get => _allDialogsVisible;
        set => SetProperty(ref _allDialogsVisible, value);
    }

    bool IRootContent.IsModal => false;

    protected virtual void UpdateState()
    {
        OnPropertyChanged(nameof(HasContent));
        OnPropertyChanged(nameof(HasVisibleContent));
        OnPropertyChanged(nameof(HasHiddenContent));
        OnPropertyChanged(nameof(RootMessagesVisible));
        HomeCommand.NotifyCanExecuteChanged();
    }
}