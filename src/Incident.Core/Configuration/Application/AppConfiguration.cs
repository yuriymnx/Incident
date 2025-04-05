using Incident.Common;
using Incident.Core.Configuration.Interfaces;
using Microsoft.Extensions.Options;

namespace Incident.Core.Configuration.Application;

public class AppConfiguration : IAppConfiguration
{
    public AppConfiguration(IOptions<AppSettings> settings)
    {
        LogLevel = string.IsNullOrEmpty(settings.Value.LogLevel) ? Constants.DefaultLogLevel : settings.Value.LogLevel;
    }

    public string LogLevel { get; set; }
}