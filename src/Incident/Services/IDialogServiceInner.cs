using System.Threading.Tasks;
using Incident.Common.Types;
using Incident.ViewModels.Base;

namespace Incident.Services;

internal interface IDialogService
{
    Task<object?> ShowRootMessageAsync(MessageInfo info);
    Task<object?> ShowDialogMessageAsync(MessageInfo info);
    void ShowWaitMessageAsync(string text, bool isModal);
    void RemoveRootMessage();
    void RemoveDialogMessage();
    Task<T> ShowRootMessageAsync<T>(IRootMessage<T> rootContent);
    Task<T> ShowDialogMessageAsync<T>(IRootMessage<T> dialogContent, bool removeOnResult = true);
    void ReplaceRootMessage(IRootContent message);
    void ReplaceDialogMessage(IRootContent message);
}