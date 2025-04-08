using System;
using System.Threading.Tasks;
using Incident.Common.Services.Interfaces;
using Incident.Common.Types;
using Incident.ViewModels.Base;
using Incident.ViewModels.Root;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

    public Task<object?> ShowRootMessageAsync(MessageInfo info) => ShowDialogMessageAsync(new MessageInfoVm(info));

    public Task<object?> ShowDialogMessageAsync(MessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    public void ShowWaitMessageAsync(string text, bool isModal)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveRootMessage()
    {
        throw new System.NotImplementedException();
    }

    public void RemoveDialogMessage()
    {
        throw new System.NotImplementedException();
    }
}