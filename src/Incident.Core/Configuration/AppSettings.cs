using Incident.Common;

namespace Incident.Core.Configuration;

public class AppSettings
{
    public string LogLevel { get; set; } = Constants.DefaultLogLevel;
}