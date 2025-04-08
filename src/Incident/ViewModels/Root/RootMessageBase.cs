using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Incident.ViewModels.Base;

namespace Incident.ViewModels.Root;

internal abstract class RootMessageBase<T> : ObservableObject, IRootMessage<T>
{
    public bool IsModal { get; protected set; }

    public async Task<T> GetResultAsync() => await CompletionSource.Task;

    protected TaskCompletionSource<T> CompletionSource { get; private set; } = new();

    protected void SetResult(T result) => CompletionSource.SetResult(result);

    public void Reuse()
    {
        if (!CompletionSource.Task.IsCompleted)
            throw new InvalidOperationException("Нельзя переиспользовать до завершения");
        CompletionSource = new TaskCompletionSource<T>();
    }
}

internal abstract class RootMessageBase : RootMessageBase<object?>, IRootMessage
{

}
