using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Incident.Composition;

internal interface IViewLocator : IDataTemplate
{
    Control Build<T>() where T : notnull;
    bool Match<T>() where T : notnull;
}