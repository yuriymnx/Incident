using System.Threading.Tasks;
using Incident.Common.Services.Interfaces;
using Incident.ViewModels.Base;

namespace Incident.Services;

internal interface IDialogServiceInner : IDialogService
{
    Task<T> ShowRootMessageAsync<T>(IRootMessage<T> rootContent);
    Task<T> ShowDialogMessageAsync<T>(IRootMessage<T> dialogContent, bool removeOnResult = true);
}