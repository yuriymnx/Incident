using System.Threading.Tasks;

namespace Incident.ViewModels.Base;

internal interface IRootContent
{
    bool IsModal { get; }

}
internal interface IRootMessage<T> : IRootContent
{
    Task<T> GetResultAsync();
}

interface IRootMessage : IRootMessage<object?>
{
}