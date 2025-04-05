using Incident.ViewModels.Base;
using Microsoft.Extensions.Logging;

namespace Incident.ViewModels;

internal class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public MainViewModel(ILogger logger) : base(logger)
    {
    }
}
