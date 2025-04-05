using System.Threading.Tasks;

namespace Incident.ViewModels.Base;

internal interface IMainViewModel
{
    Task Startup(string[]? args);
    public void ShowDialogMessage(IRootContent message);

    public void ReplaceDialogMessage(IRootContent message);

    public void RemoveDialogMessage();

    public void ShowRootMessage(IRootContent message);

    public void ReplaceRootMessage(IRootContent message);

    public void RemoveRootMessage();

    IRootContent? TopDialogMessage { get; }

    /// <summary>
    /// В случае false виден только самый верхний диалог
    /// </summary>
    bool AllDialogsVisible { get; set; }
}