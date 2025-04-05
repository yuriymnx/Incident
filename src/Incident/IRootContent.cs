using System.Threading.Tasks;

namespace Incident;

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