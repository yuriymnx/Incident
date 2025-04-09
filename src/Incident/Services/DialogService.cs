using Incident.Common.Services.Interfaces;
using Incident.Common.Types;
using Incident.ViewModels.Base;
using Incident.ViewModels.Root;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Incident.Services;

internal class DialogService : IDialogService
{
    private readonly ILogger<DialogService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly Lazy<IMainViewModel> _lazyMessageOwner;
    public DialogService(
        ILogger<DialogService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _lazyMessageOwner = new Lazy<IMainViewModel>(() => _serviceProvider.GetRequiredService<IMainViewModel>());
    }

    public Task<object?> ShowRootMessageAsync(MessageInfo info) => ShowRootMessageAsync(new MessageInfoVm(info));
    public Task<object?> ShowDialogMessageAsync(MessageInfo info) => ShowDialogMessageAsync(new MessageInfoVm(info));

    public async Task<T> ShowDialogMessageAsync<T>(IRootMessage<T> message, bool removeOnResult = true)
    {
        _lazyMessageOwner.Value.ShowDialogMessage(message);
        var result = await message.GetResultAsync();
        if (removeOnResult)
            RemoveDialogMessage();
        return result;
    }

    public async Task<T> ShowRootMessageAsync<T>(IRootMessage<T> message)
    {
        ReplaceRootMessage(message);
        var result = await message.GetResultAsync();
        RemoveRootMessage();
        return result;
    }

    public void ReplaceRootMessage(IRootContent message) => _lazyMessageOwner.Value.ReplaceRootMessage(message);
    public void ReplaceDialogMessage(IRootContent message) => _lazyMessageOwner.Value.ReplaceDialogMessage(message);
    public void ShowWaitMessageAsync(string text, bool isModal) => _lazyMessageOwner.Value.ShowRootMessage(new WaitVm(text, isModal));
    public void RemoveRootMessage() => _lazyMessageOwner.Value.RemoveRootMessage();
    public void RemoveDialogMessage() => _lazyMessageOwner.Value.RemoveDialogMessage();
}