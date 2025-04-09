using Incident.ViewModels.Base;

namespace Incident.ViewModels.Root;

internal class WaitVm : IRootContent
{
    public string Text { get; }
    public bool IsModal { get; }
    public WaitVm(string text, bool isModal)
    {
        Text = text;
        IsModal = isModal;
    }

}