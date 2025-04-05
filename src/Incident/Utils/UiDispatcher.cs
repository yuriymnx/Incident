using Avalonia.Threading;
using System.Threading.Tasks;

namespace Incident.Utils;

internal static class UiDispatcher
{
    public static async Task Yield(DispatcherPriority? priority = null)
    {
        await Yield(1, priority);
    }
    public static async Task Yield(int delay, DispatcherPriority? priority = null)
    {
        await Dispatcher.UIThread.AwaitWithPriority(Task.Delay(delay), priority ?? DispatcherPriority.ContextIdle);
    }
}