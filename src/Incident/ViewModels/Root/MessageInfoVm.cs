using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Incident.Common.Types;
using Incident.ViewModels.Base;

namespace Incident.ViewModels.Root;

internal class MessageInfoVm : RootMessageBase, IRootMessage
{
    private Action? _action;
    public string? Header { get; protected set; }
    public string? Message { get; protected set; }
    public new bool IsModal { get; protected set; }
    public MessageInfoVm(MessageInfo messageInfo)
    {
        Header = messageInfo.Header;
        Message = messageInfo.Message;
        IsModal = messageInfo.IsModal;
        _action = messageInfo.Action;
        SetResultCommand = new AsyncRelayCommand<object?>(SetResultAsync);
    }

    private async Task SetResultAsync(object? param)
    {
        if (param is not MessageInfoActionVm vm)
            return;
        var result = vm.Result;
        if (result is not Func<Task<object>> asyncFunc)
        {
            switch (result)
            {
                case Action action:
                    action();
                    break;
                case Func<object> func:
                    result = func();
                    break;
            }
            SetResult(result);
            return;
        }

        try
        {
            vm.InProcess = true;
            SetResult(await asyncFunc());
        }
        finally
        {
            vm.InProcess = false;
        }

    }

    public ICommand SetResultCommand { get; protected set; }

    public override string ToString() => $"Header = {Header}, text = {Message}";

    public void Clicked(string param)
    {
        _action?.Invoke();
    }
}


internal class MessageInfoActionVm : ObservableObject
{
    public MessageInfoActionVm(string text, object result)
    {
        Text = text;
        Result = result;
    }

    public string Text { get; protected set; }
    public object Result { get; protected set; }

    private bool _inProcess;

    public bool InProcess
    {
        get => _inProcess;
        set => SetProperty(ref _inProcess, value);
    }
}



