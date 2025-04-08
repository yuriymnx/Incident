using System;
using System.Threading.Tasks;
using Incident.ViewModels.Base;
using Microsoft.Extensions.Logging;

namespace Incident.ViewModels;

internal class WelcomeViewModel : MainViewModelBase , IDisposable
{
    public WelcomeViewModel( ILogger<WelcomeViewModel> logger) : base(logger)
    {
    }

    public override Task Startup(string[]? args)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
