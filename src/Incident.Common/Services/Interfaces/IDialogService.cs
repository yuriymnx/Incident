using Incident.Common.Types;

namespace Incident.Common.Services.Interfaces;

public interface IDialogService
{
    Task<object?> ShowRootMessageAsync(MessageInfo info);
    Task<object?> ShowDialogMessageAsync(MessageInfo info);
    void ShowWaitMessageAsync(string text, bool isModal);
    void RemoveRootMessage();
    void RemoveDialogMessage();

}