using Incident.Common;
using Microsoft.Extensions.Options;

namespace Incident.Core.Configuration;

public class AppConfiguration : IAppConfiguration
{
    public AppConfiguration(IOptions<AppSettings> settings)
    {
        LogLevel = string.IsNullOrEmpty(settings.Value.LogLevel) ? Constants.DefaultLogLevel : settings.Value.LogLevel;
    }

    public string LogLevel { get; set; }
}