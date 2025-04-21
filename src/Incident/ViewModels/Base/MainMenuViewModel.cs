using Microsoft.Extensions.Logging;

namespace Incident.ViewModels.Base;

internal sealed class MainMenuViewModel
{
    private ILogger<MainMenuViewModel> _logger;
    public MainMenuViewModel(ILogger<MainMenuViewModel> logger)
    {
        _logger = logger;
    }

    public string Version => "pre_alpha_test";
}